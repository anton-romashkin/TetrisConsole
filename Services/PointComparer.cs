using System;
using System.Drawing;

namespace TetrisThird.Services;

public class PointYComparer : IComparer<Point>
{
    public int Compare(Point point1, Point point2)
    {
        return point2.Y.CompareTo(point1.Y);
    }
}

