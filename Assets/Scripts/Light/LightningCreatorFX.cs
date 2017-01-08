using UnityEngine;
using System.Collections;
using System;
using System.Linq;

public class LightningCreatorFX : MonoBehaviour
{
    private const int _MinSegmentCount = 5;
    private const int _SegmentCountBase = 2;
    private const int _SegmentPerWall = 1;

    public LightningFX LightningPrefab;

    private int _MaxLightningCount = 5;
    private int _CurLightningCount = 0;
    private float _MaxLightningSize;
    private Vector3 _LightningStartPosition;
    private int _SegmentCount = 12;

    private bool _IsInitialized = false;
    public bool IsEnabled;

    public void Init(GameObject beam, int wallCount)
    {
        Renderer beamRenderer = beam.GetComponent<Renderer>();
        _SegmentCount = Mathf.Max(_MinSegmentCount, _SegmentCountBase + (wallCount * _SegmentPerWall));

        float beamHeight = beamRenderer.bounds.size.y;
        float beamWidth = beamRenderer.bounds.size.x;

        bool isBeamHorizontal = beam.transform.rotation.z == 0;
        _MaxLightningSize = (isBeamHorizontal ? beamWidth : beamHeight);

        _LightningStartPosition = new Vector3();
        _LightningStartPosition.x -= _MaxLightningSize * 0.5f;

        _IsInitialized = true;
    }

    void Update()
    {
        if (!_IsInitialized)
            return;

        if (transform.childCount < _MaxLightningCount)
            CreateLightning();
    }

    private void CreateLightning()
    {
        var instance1 = (LightningFX)Instantiate(LightningPrefab, transform.position, transform.parent.rotation);
        instance1.transform.SetParent(transform);
        instance1.MaxX = _MaxLightningSize;
        instance1.StartPosition = _LightningStartPosition;
        instance1.SegmentCount = _SegmentCount;
    }
}
