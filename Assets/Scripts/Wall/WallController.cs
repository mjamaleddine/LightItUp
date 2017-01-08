using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

public class WallController : MonoBehaviour
{

    #region Debug

    public static bool IsDisabled = false;

    #endregion

    private LevelManager CurrentLevel
    {
        get
        {
            return GameManager.Instance.CurrentLevel;
        }
    }

    /// <summary>
    /// Up
    /// </summary>
    public bool IsHitEdgeA { get; private set; }
    /// <summary>
    /// Left
    /// </summary>
    public bool IsHitEdgeB { get; private set; }
    /// <summary>
    /// Down
    /// </summary>
    public bool IsHitEdgeC { get; private set; }
    /// <summary>
    /// Right
    /// </summary>
    public bool IsHitEdgeD { get; private set; }

    public bool CanHitEdgeA { get; private set; }
    public bool CanHitEdgeB { get; private set; }
    public bool CanHitEdgeC { get; private set; }
    public bool CanHitEdgeD { get; private set; }
    
    public Edge EdgeA { get; private set; }
    public Edge EdgeB { get; private set; }
    public Edge EdgeC { get; private set; }
    public Edge EdgeD { get; private set; }

    public Vector2 CornerA { get { return EdgeA.Start; } }
    public Vector2 CornerB { get { return EdgeB.Start; } }
    public Vector2 CornerC { get { return EdgeC.Start; } }
    public Vector2 CornerD { get { return EdgeD.Start; } }

    private Collider2D _WallCollider;

    public Collider2D WallCollider
    {
        get
        {
            if (_WallCollider == null)
                _WallCollider = GetComponent<Collider2D>();

            return _WallCollider;
        }
    }

    public WallContainerController ParentWallContainer { get; internal set; }

    private static readonly int WallLayerMask;

    internal Edge GetEdge(EdgeKind kind)
    {
        if (kind == EdgeKind.A)
            return EdgeA;
        if (kind == EdgeKind.B)
            return EdgeB;
        if (kind == EdgeKind.C)
            return EdgeC;

        return EdgeD;
    }

    internal EdgeKind GetEdgeKind(Vector2 point)
    {
        if (EdgeA.Contains(point))
            return EdgeKind.A;
        if (EdgeB.Contains(point))
            return EdgeKind.B;
        if (EdgeC.Contains(point))
            return EdgeKind.C;
        if (EdgeD.Contains(point))
            return EdgeKind.D;

        return EdgeKind.None;
    }

    internal void DrawEdges()
    {
        Debug.DrawLine(EdgeA.Start, EdgeA.End, Color.green, 180);
        Debug.DrawLine(EdgeB.Start, EdgeB.End, Color.green, 180);
        Debug.DrawLine(EdgeC.Start, EdgeC.End, Color.green, 180);
        Debug.DrawLine(EdgeD.Start, EdgeD.End, Color.green, 180);
    }

    static WallController()
    {
        WallLayerMask = LayerMask.GetMask("Wall");
    }

    /// <summary>
    /// To be called from start() in levelmanager to make sure the wallSectionCount can be set in one place.
    /// </summary>
    internal void InitSections()
    {
        if (IsDisabled)
            return;

        var bounds = WallCollider.bounds;

        var rotation = Quaternion.AngleAxis(90, Vector3.forward);
        Vector3 cornerA = bounds.center + bounds.extents;
        Vector3 cornerB = bounds.center + (rotation * bounds.extents);
        Vector3 cornerC = bounds.center - bounds.extents;
        Vector3 cornerD = bounds.center - (rotation * bounds.extents);

        CanHitEdgeA = !IsEdgeBlockedByWall(cornerA, cornerB);
        CanHitEdgeB = !IsEdgeBlockedByWall(cornerB, cornerC);
        CanHitEdgeC = !IsEdgeBlockedByWall(cornerC, cornerD);
        CanHitEdgeD = !IsEdgeBlockedByWall(cornerD, cornerA);

        EdgeA = new Edge(cornerA, cornerB);
        EdgeB = new Edge(cornerB, cornerC);
        EdgeC = new Edge(cornerC, cornerD);
        EdgeD = new Edge(cornerD, cornerA);

        //DrawEdges();
    }

    /// <summary>
    /// TODO rather check if any light is hitting an edge
    /// </summary>
    private bool IsEdgeBlockedByWall(Vector3 cornerA, Vector3 cornerB)
    {
        Vector3 edgeVector = cornerB - cornerA;
        Vector3 edgeCenter = cornerA + (edgeVector * 0.5f);

        Vector3 wallCenter = WallCollider.bounds.center;
        Vector3 wallCenterToEdgeCenter = edgeCenter - wallCenter;

        //go a little outside this wall to check if another wall is placed next to it
        float rayLength = wallCenterToEdgeCenter.magnitude * 1.5f;
        var hits = Physics2D.RaycastAll(wallCenter, wallCenterToEdgeCenter, rayLength, WallLayerMask);
        bool hasHitOtherWall = hits.Any(cur => cur.collider.gameObject != gameObject);

        return hasHitOtherWall;
    }

    internal void UpdateEdges(IEnumerable<LightController> enabledLights)
    {
        if (IsDisabled)
            return;

        foreach (var light in enabledLights)
        {
            var lightHits = light.LightBeam.GetHits(this);
            if (lightHits == null)
                continue;

            var hitSegmentsEdgeA = lightHits.GetHitSegments(EdgeKind.A);
            var hitSegmentsEdgeB = lightHits.GetHitSegments(EdgeKind.B);
            var hitSegmentsEdgeC = lightHits.GetHitSegments(EdgeKind.C);
            var hitSegmentsEdgeD = lightHits.GetHitSegments(EdgeKind.D);

            foreach (var segment in hitSegmentsEdgeA)
                Debug.DrawLine(segment.Start, segment.End, Color.white, 180);
            foreach (var segment in hitSegmentsEdgeB)
                Debug.DrawLine(segment.Start, segment.End, Color.white, 180);
            foreach (var segment in hitSegmentsEdgeC)
                Debug.DrawLine(segment.Start, segment.End, Color.white, 180);
            foreach (var segment in hitSegmentsEdgeD)
                Debug.DrawLine(segment.Start, segment.End, Color.white, 180);
        }

    }

    [Conditional("DEBUG")]
    private static void ExecuteIfDEBUG(bool condition, Action action)
    {
        if (condition && action != null)
            action();
    }

}
