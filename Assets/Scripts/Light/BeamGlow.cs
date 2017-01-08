using UnityEngine;
using System.Collections;

public class BeamGlow : MonoBehaviour
{

    public Color ColorStart = Color.red;
    public Color ColorEnd = Color.green;
    public float Duration = 1.0F;

    private LineRenderer _line;

    // Use this for initialization
    void Start()
    {
        _line = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        float lerp = Mathf.PingPong(Time.time, Duration) / Duration;
        _line.SetColors(Color.Lerp(ColorStart, ColorEnd, lerp), Color.Lerp(ColorStart, ColorEnd, lerp));
    }
}
