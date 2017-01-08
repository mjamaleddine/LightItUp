using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EdgeHitContainer
{
    private bool _isClosed = false;

    private List<Vector2> _HitsEdgeA = new List<Vector2>();
    private List<Vector2> _HitsEdgeB = new List<Vector2>();
    private List<Vector2> _HitsEdgeC = new List<Vector2>();
    private List<Vector2> _HitsEdgeD = new List<Vector2>();

    internal void Add(EdgeKind edge, Vector2 point)
    {
        if (_isClosed)
            return;

        if (edge == EdgeKind.A)
            _HitsEdgeA.Add(point);
        else if (edge == EdgeKind.B)
            _HitsEdgeB.Add(point);
        else if (edge == EdgeKind.C)
            _HitsEdgeC.Add(point);
        else if (edge == EdgeKind.D)
            _HitsEdgeD.Add(point);
    }

    public IEnumerable<LineSegment> GetHitSegments(EdgeKind edge)
    {
        if (edge == EdgeKind.A)
            return GetSegments(_HitsEdgeA);
        else if (edge == EdgeKind.B)
            return GetSegments(_HitsEdgeB);
        else if (edge == EdgeKind.C)
            return GetSegments(_HitsEdgeC);
        else if (edge == EdgeKind.D)
            return GetSegments(_HitsEdgeD);

        return Enumerable.Empty<LineSegment>();
    }

    /// <summary>
    /// Close container for editing and verify even edge hit counts
    /// </summary>
    public void CloseContainer()
    {
        if (_isClosed)
            return;

        _isClosed = true;

        EvenHits(_HitsEdgeA);
        EvenHits(_HitsEdgeB);
        EvenHits(_HitsEdgeC);
        EvenHits(_HitsEdgeD);
    }

    private void EvenHits(List<Vector2> hits)
    {
        if (hits.Count == 1)
            hits.Clear();
        if (hits.Count % 2 == 0)
            return;

        //for now, check only if first is too close to last one, might happen with first and last ray of the shadow scan
        //if that's ever not enough, do it more properly!

        var a = hits.First();
        var b = hits.ElementAt(1);
        var c = hits.Last();

        float ab = Vector2.Distance(a, b);
        float cb = Vector2.Distance(c, b);

        //keep the point that's farther to the edge (more distance to point B)
        if (ab > cb)
            hits.Remove(c);
        else
            hits.Remove(a);
    }

    private IEnumerable<LineSegment> GetSegments(List<Vector2> points)
    {
        if (!_isClosed)
            throw new InvalidOperationException("Container must be closed to calculate segments!");

        for (int i = 0; i < points.Count; i += 2)
        {
            var pointA = points[i];
            var pointB = points[i + 1];

            yield return new LineSegment(pointA, pointB);
        }
    }

}