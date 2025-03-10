using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using UnityEngine.Assertions;
using static DirectionUtil;

class Util
{
    public static void DrawRect(Rect rect, Color color)
    {
        Vector3[] points = new Vector3[]
        {
            new Vector3(rect.xMin, rect.yMin),
            new Vector3(rect.xMin, rect.yMax),
            new Vector3(rect.xMax, rect.yMax),
            new Vector3(rect.xMax, rect.yMin),
        };
        Gizmos.color = color;
        Gizmos.DrawLineStrip(points, true);
    }
}