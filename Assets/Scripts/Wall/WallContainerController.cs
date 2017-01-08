using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class WallContainerController : MonoBehaviour
{

    public GameObject Beam;
    public LightningCreatorFX LightningCreator;

    private List<WallController> _Walls;

    public int WallCount
    {
        get
        {
            if (_Walls == null)
                return 0;

            return _Walls.Count;
        }
    }

    void Start()
    {
        InitWalls();
        InitBeam();
    }

    private void InitBeam()
    {
        Bounds wallContainerBounds = _Walls.First().WallCollider.bounds;
        for (int i = 1; i < _Walls.Count; i++)
            wallContainerBounds.Encapsulate(_Walls[i].WallCollider.bounds);

        //center beam on walls
        Vector3 beamCenter = wallContainerBounds.center;
        beamCenter.z = Beam.transform.position.z;

        Beam.transform.position = beamCenter;

        //size beam to walls
        var beamLine = Beam.GetComponent<LineRenderer>();
        float maxWallContainerSizeHalf = Mathf.Max(wallContainerBounds.size.x, wallContainerBounds.size.y) * 0.5f;
        beamLine.SetPosition(0, new Vector3(-maxWallContainerSizeHalf, 0f, 0f));
        beamLine.SetPosition(1, new Vector3(maxWallContainerSizeHalf, 0f, 0f));

        //init lightning creator
        LightningCreator.Init(Beam, _Walls.Count);
    }

    private void InitWalls()
    {
        //init contained walls
        _Walls = transform.GetComponentsInChildren<WallController>().ToList();
        foreach (var wall in _Walls)
            wall.ParentWallContainer = this;
    }

    private void TurnOn()
    {
        Beam.SetActive(true);
    }

    public void TurnOff()
    {
        Beam.SetActive(false);
    }

    internal void UpdateBeam()
    {
        bool allLitEdgeA = _Walls.All(cur => cur.IsHitEdgeA);
        bool allLitEdgeB = _Walls.All(cur => cur.IsHitEdgeB);
        bool allLitEdgeC = _Walls.All(cur => cur.IsHitEdgeC);
        bool allLitEdgeD = _Walls.All(cur => cur.IsHitEdgeD);

        if (allLitEdgeA || allLitEdgeB || allLitEdgeC || allLitEdgeD)
        {
            TurnOn();
        }
        else if (!allLitEdgeA && !allLitEdgeB && !allLitEdgeC && !allLitEdgeD)
        {
            TurnOff();
        }
    }
}
