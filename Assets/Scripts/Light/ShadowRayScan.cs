using UnityEngine;
using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;

/// <summary>
/// see http://unitycoder.com/blog/2012/01/04/fake-realtime-raycast-shadows-unity3d/
/// </summary>
public class ShadowRayScan : MonoBehaviour
{
    private static readonly int WallLayerMask;

    //1280
    //5120
    //10240
    //64
    private int _raysToShoot = 10240;
    private int _distance = 15;
    private Mesh _mesh;

    private WallController _curentlyIgnoredWall;
    private Dictionary<WallController, EdgeHitContainer> _WallHits = new Dictionary<WallController, EdgeHitContainer>();

    static ShadowRayScan()
    {
        WallLayerMask = LayerMask.GetMask("Wall");
    }

    public void InitLightMesh(IEnumerable<WallController> wallControllers)
    {
        _mesh = GetComponent<MeshFilter>().mesh;

        var vertices2d = new List<Vector2>();
        GameObject lastHitWall = null;
        WallController lastWallController = null;
        EdgeKind lastHitEdgeKind = EdgeKind.None;
        Vector3 lastHitPointLocal = new Vector3();
        Vector3 lastHitPointWorld = new Vector3();

        DrawRaycast(transform.position, wallControllers);

        float angle = 0;
        for (int i = 0; i < _raysToShoot; i++)
        {
            var x = Mathf.Sin(angle);
            var y = Mathf.Cos(angle);
            angle -= 2 * Mathf.PI / _raysToShoot;

            Vector3 dir = new Vector3(x, y, 0);
            RaycastHit2D hit;
            hit = Physics2D.Raycast(transform.position, dir, _distance, WallLayerMask);
            if (hit.collider != null)
            {
                Vector3 curHitPointWorld = hit.point;
                Vector3 curHitPointLocal = transform.InverseTransformPoint(curHitPointWorld);

                WallController curWallController = hit.collider.GetComponent<WallController>();
                EdgeKind curHitEdgeKind = curWallController.GetEdgeKind(curHitPointWorld);

                if (curHitEdgeKind == EdgeKind.None)
                {
                    //TODO this shouldn't happen!! this happens with all the points close to an intersecting point between two edges
                    //that's why the edges don't close properly

                    lastHitPointLocal = curHitPointLocal;
                    continue;

                    //draw points on if on no edge
                    //Debug.DrawLine(transform.position, hit.point, Color.white, 180);
                    //curWallController.DrawEdges();
                }

                if (lastHitWall == null)
                {
                    lastHitWall = hit.collider.gameObject;
                }
                else if (lastHitWall != hit.collider.gameObject || curHitEdgeKind != lastHitEdgeKind)
                {
                    //always add first and last hitpoint on the same wall to form an edge

                    //add last hit point
                    vertices2d.Add(lastHitPointLocal);

                    lastHitWall = hit.collider.gameObject;

                    //add current hit point
                    vertices2d.Add(curHitPointLocal);

                    AddWallHit(lastWallController, lastHitEdgeKind, lastHitPointWorld);

                    bool isCurWallParent = GetComponentInParent<WallController>() == curWallController;
                    if (isCurWallParent)
                    {
                        var curEdge = curWallController.GetEdge(curHitEdgeKind);
                        AddWallHit(curWallController, curHitEdgeKind, curEdge.Start);
                        AddWallHit(curWallController, curHitEdgeKind, curEdge.End);

                        _curentlyIgnoredWall = curWallController;
                    }
                    else
                        AddWallHit(curWallController, curHitEdgeKind, curHitPointWorld);
                }

                lastHitPointLocal = curHitPointLocal;
                lastHitPointWorld = curHitPointWorld;

                lastHitEdgeKind = curHitEdgeKind;
                lastWallController = curWallController;
            }
            else
            {
                var tmp = transform.InverseTransformPoint(transform.position + dir);
                lastHitPointLocal = new Vector2(tmp.x, tmp.y);
                vertices2d.Add(lastHitPointLocal);
            }
        }
        vertices2d.Add(lastHitPointLocal);

        // build mesh
        List<Vector3> newVertices = new List<Vector3>();
        foreach (var curPos in vertices2d)
        {
            newVertices.Add(new Vector3(curPos.x, curPos.y, 0));
        }

        var triangles = new int[newVertices.Count * 3];

        // triangle list
        int j = -1;
        for (int n = 0; n < triangles.Length - 3; n += 3)
        {
            j++;
            triangles[n] = newVertices.Count - 1;
            if (j >= newVertices.Count)
            {
                triangles[n + 1] = 0;
            }
            else
            {
                triangles[n + 1] = j + 1;
            }
            triangles[n + 2] = j;
        }
        j++;

        // central point
        newVertices[newVertices.Count - 1] = new Vector3(0, 0, 0);
        triangles[triangles.Length - 3] = newVertices.Count - 1;
        triangles[triangles.Length - 2] = 0;
        triangles[triangles.Length - 1] = j - 1;

        // Create the mesh
        _mesh.vertices = newVertices.ToArray();
        _mesh.triangles = triangles;
        _mesh.uv = new Vector2[newVertices.Count];

        CloseWallHitContainers();
    }

    private void DrawRaycast(Vector2 lightPosition, IEnumerable<WallController> wallControllers)
    {
        Vector2 horizontalLightLine = lightPosition - Vector2.right;
        Vector2 verticalLightLine = lightPosition - Vector2.down;

        foreach (var wall in wallControllers)
        {
            bool canSeeLightA = CanSeeLight(lightPosition, wall.CornerA);
            bool canSeeLightB = CanSeeLight(lightPosition, wall.CornerB);
            bool canSeeLightC = CanSeeLight(lightPosition, wall.CornerC);
            bool canSeeLightD = CanSeeLight(lightPosition, wall.CornerD);

            var lightToA = wall.CornerA - lightPosition;
            var lightToB = wall.CornerB - lightPosition;
            var lightToC = wall.CornerC - lightPosition;
            var lightToD = wall.CornerD - lightPosition;

            if (canSeeLightA && Vector2.Angle(horizontalLightLine, lightToA) < 1)
            {
                AddWallHit(wall, EdgeKind.A, wall.CornerA);
                //AddWallHit(wall, EdgeKind.D, wall.CornerA);
            }
            if (canSeeLightB && Vector2.Angle(horizontalLightLine, lightToB) < 1)
            {
                AddWallHit(wall, EdgeKind.B, wall.CornerB);
                //AddWallHit(wall, EdgeKind.C, wall.CornerB);
            }
            if (canSeeLightC && Vector2.Angle(horizontalLightLine, lightToC) < 1)
            {
                AddWallHit(wall, EdgeKind.C, wall.CornerC);
                //AddWallHit(wall, EdgeKind.D, wall.CornerC);
            }
            if (canSeeLightD && Vector2.Angle(horizontalLightLine, lightToD) < 1)
            {
                AddWallHit(wall, EdgeKind.D, wall.CornerD);
            }
        }



        //var hit = Physics2D.Raycast(transform.position, direction, _distance, WallLayerMask);
        //if (hit.collider == null)
        //    return;

        //Debug.DrawLine(transform.position, hit.point, Color.green, 180);

        //foreach (var wall in wallControllers)
        //{
        //    var edgeA = wall.EdgeA.StartToEnd;
        //    var edgeB = wall.EdgeB.StartToEnd;
        //    var edgeC = wall.EdgeC.StartToEnd;
        //    var edgeD = wall.EdgeD.StartToEnd;

        //    Vector2.inter(
        //}
    }

    private bool CanSeeLight(Vector2 light, Vector2 point)
    {
        var hit = Physics2D.Raycast(light, point - light, WallLayerMask);
        return hit.collider == null;
    }

    private void CloseWallHitContainers()
    {
        foreach (var pair in _WallHits)
            pair.Value.CloseContainer();
    }

    private void AddWallHit(WallController wallController, EdgeKind edgeKind, Vector2 point)
    {
        if (wallController == _curentlyIgnoredWall)
            return;

        EdgeHitContainer curEdgeHits = null;
        if (!_WallHits.TryGetValue(wallController, out curEdgeHits))
        {
            curEdgeHits = new EdgeHitContainer();
            _WallHits.Add(wallController, curEdgeHits);
        }

        curEdgeHits.Add(edgeKind, point);
    }

    public EdgeHitContainer GetHits(WallController wall)
    {
        EdgeHitContainer curEdgeHits = null;
        if (_WallHits.TryGetValue(wall, out curEdgeHits))
            return curEdgeHits;

        return null;
    }

}
