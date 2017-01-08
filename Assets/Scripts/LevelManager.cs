using UnityEngine;
using System.Collections;
using System.Diagnostics;
using UnityEngine.UI;
using System;
using System.Linq;
using System.Collections.Generic;
using Debug = UnityEngine.Debug;

public class LevelManager : MonoBehaviour
{

    #region Debug Variables

    public bool HighlightLitEdges = false;
    public bool HighlightVisibleEdges = false;

    #endregion

    private bool _isHoldingFireDown = false;

    public float LightTouchRadius = 1;
    public float WallSectionCount = 100;

    private List<LightController> _LightControllers;
    private List<WallController> _WallControllers;

    private bool CanPlaceLight
    {
        get
        {
            return true;
        }
    }

    void Start()
    {
        GameManager.Instance.CurrentLevel = this;

        InitWalls();
        InitLights();
    }

    private void InitWalls()
    {
        _WallControllers = FindObjectsOfType<WallController>().ToList();
        foreach (var wall in _WallControllers)
            wall.InitSections();
    }

    private void InitLights()
    {
        _LightControllers = FindObjectsOfType<LightController>().ToList();
        foreach (var light in _LightControllers)
            light.Init(_WallControllers);
    }

    void Update()
    {
        bool fireDown = Input.GetButton("Fire1");
        if (fireDown && !_isHoldingFireDown)
        {
            _isHoldingFireDown = true;

            var mousePosition = Input.mousePosition;
            mousePosition.z = 10;

            var worldMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            var nearestLight = _LightControllers.OrderBy(cur => cur.DistanceToMouse(worldMousePosition)).FirstOrDefault();

            if (nearestLight != null &&
                (CanPlaceLight || nearestLight.IsLightEnabled) &&
                nearestLight.DistanceToMouse(worldMousePosition) < LightTouchRadius)
            {
                nearestLight.Toggle();

                UpdateWallHits();
                //UpdateLightCount();

                //CheckLevelFinished();
            }
        }
        else if (!fireDown)
        {
            _isHoldingFireDown = false;
        }
    }
    private void UpdateWallHits()
    {
        var enabledLights = _LightControllers.Where(cur => cur.IsLightEnabled).ToList();
        foreach (var wall in _WallControllers)
            wall.UpdateEdges(enabledLights);
    }

}
