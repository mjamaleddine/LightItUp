using UnityEngine;
using System.Collections;

public class LightningFX : MonoBehaviour
{
    public float PosRange = 0.15f;

    private LineRenderer _lineRenderer;
    private Color _color = Color.white;
    private float _radius = 1f;

    public float MaxX { get; set; }
    public Vector3 StartPosition { get; set; }
    public int SegmentCount { get; set; }

    public LightningFX()
    {
        MaxX = 1f;
        StartPosition = new Vector3();
        SegmentCount = 12;
    }

    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.SetVertexCount(SegmentCount + 1);

        float segmentOffset = MaxX / (SegmentCount - 1);
        float previousMaxX = 0;
        for (int i = 1; i < SegmentCount; i++)
        {
            float curMinX = previousMaxX + 0.01f;
            float curMaxX = i * segmentOffset;
            _lineRenderer.SetPosition(i, StartPosition + new Vector3(Random.Range(curMinX, curMaxX), Random.Range(-PosRange, PosRange), Random.Range(-PosRange, PosRange)));

            previousMaxX = curMaxX;
        }

        _lineRenderer.SetPosition(0, StartPosition);
        _lineRenderer.SetPosition(SegmentCount, StartPosition + new Vector3(MaxX, 0f, 0f));
    }

    void Update()
    {
        _color.a -= 10f * Time.deltaTime;

        if (_color.a <= 0f)
            Destroy(gameObject);
    }
}
