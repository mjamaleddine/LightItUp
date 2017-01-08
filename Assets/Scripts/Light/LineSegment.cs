using UnityEngine;

public class LineSegment
{
    public Vector2 Start { get; private set; }
    public Vector2 End { get; private set; }

    public LineSegment(Vector2 start, Vector3 end)
    {
        Start = start;
        End = end;
    }

}