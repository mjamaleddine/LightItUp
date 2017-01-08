using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class Edge
{
    public Vector2 Start { get; private set; }
    public Vector2 End { get; private set; }

    public Vector2 StartToEnd { get; private set; }

    public Edge(Vector2 start, Vector2 end)
    {
        Start = start;
        End = end;

        StartToEnd = end - start;
    }

    /// <summary>
    /// see http://www.lucidarme.me/?p=1952
    /// </summary>
    public bool Contains(Vector2 point)
    {
        Vector3 AB = StartToEnd;
        Vector3 AC = point - Start;

        //check is on line
        //bool isPointOnLine = Vector3.Cross(AB, AC).magnitude <= Mathf.Epsilon;
        bool isPointOnLine = Vector3.Angle(AB, AC) <= Mathf.Epsilon;
        if (!isPointOnLine)
            return false;

        //check is between points
        float kAC = Vector3.Dot(AB, AC);
        float kAB = Vector3.Dot(AB, AB);

        return kAC >= 0 && kAC <= kAB;
    }

}