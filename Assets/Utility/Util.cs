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

    public static T[] ClonedElementArray<T>(int count, T obj) where T : ICloneable
    {
        T[] result = new T[count];
        for(int i = 0; i < result.Length; i++)
        {
            result[i] = (T)obj.Clone();
        }
        return result;
    }

    public static T[] DefaultedArray<T>(int count) where T : new()
    {
        T[] result = new T[count];
        for(int i = 0; i < result.Length; i++)
        {
            result[i] = new T();
        }
        return result;
    }
}