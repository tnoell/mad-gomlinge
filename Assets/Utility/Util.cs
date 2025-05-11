using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;
using static DirectionUtil;
using System.Linq;
using System.Reflection;

public class Util
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

    public static List<T> DeepCopyList<T>(List<T> source)
    {
        List<T> copy = new List<T>();
        foreach (T elem in source)
        {
            copy.Add((T)DeepCopy(elem));
        }
        return copy;
    }

    public static List<T> ShallowCopyList<T>(List<T> source)
    {
        List<T> copy = new List<T>();
        foreach (T elem in source)
        {
            copy.Add((T)elem);
        }
        return copy;
    }

    public static T[] ShallowCopyArray<T>(T[] source)
    {
        T[] copy = new T[source.Length];
        for (int i = 0; i < source.Length; i++)
        {
            copy[i] = source[i];
        }
        return copy;
    }

    public static List<Type> GetSubTypes(Type type)
    {
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
        List<Type> types = new List<Type>();
        foreach (Assembly assembly in assemblies)
        {
            types.AddRange(assembly.GetTypes());
        }
        return types.Where(t => t.IsSubclassOf(type)).ToList();
    }
    
    public static object DeepCopy(object source)
    {
        if (source == null) return null;
        Type type = source.GetType();
        if (type.IsPrimitive) return source; // don't value-copy structs, because they can contain members that need to be memberwise-copied
        if (type.IsGenericType &&
            type.GetGenericTypeDefinition() == typeof(List<>))
        {
            return DeepCopyUnspecifiedList(source);
        }
        if (type.IsArray)
        {
            return DeepCopyUnspecifiedArray(source);
        }
        if (type == typeof(string))
        {
            return ((string)source).Clone();
        }
        if (!IsSerializable(type))
        {
            // Debug.Log("Value copying " + fieldInfo.Name + " (" + fieldInfo.FieldType + "): " + fieldInfo.GetValue(source));
            return source;
        }
        
        return MemberwiseDeepCopy(source);
    }

    private static object DeepCopyUnspecifiedList(object source)
    {
        Type type = source.GetType();
        var listType = type.GetGenericArguments()[0];
        var originalList = source as IList;
        var listCopy = Activator.CreateInstance(typeof(List<>).MakeGenericType(listType)) as IList;
        for (int i = 0; i < originalList.Count; i++)
        {
            listCopy.Add(DeepCopy(originalList[i]));
        }
        return listCopy;
    }

    private static object DeepCopyUnspecifiedArray(object source)
    {
        Type type = source.GetType();
        Type arrayType = type.GetElementType();
        Array originalArray = source as Array;
        Array arrayCopy = Array.CreateInstance(arrayType, originalArray.Length);
        for (int i = 0; i < originalArray.Length; i++)
        {
            arrayCopy.SetValue(DeepCopy(originalArray.GetValue(i)), i);
        }
        return arrayCopy;
    }

    private static object MemberwiseDeepCopy(object source)
    {
        Type type = source.GetType();
        object copy = Activator.CreateInstance(type);
        List<FieldInfo> fieldInfos = new List<FieldInfo>();
        while (type != typeof(object))
        {
            fieldInfos.AddRange(type.GetFields(BindingFlags.Instance |
                                               BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly));
            type = type.BaseType;
        }

        foreach (var fieldInfo in fieldInfos)
        {
            bool toBeCopied = fieldInfo.IsPublic;
            var attributes = fieldInfo.CustomAttributes;
            foreach (var attribute in attributes)
            {
                if (attribute.AttributeType == typeof(SerializeField) ||
                    attribute.AttributeType == typeof(SerializeReference))
                {
                    toBeCopied = true;
                    break;
                }
            }
            if (!toBeCopied) continue;
            fieldInfo.SetValue(copy, DeepCopy(fieldInfo.GetValue(source)));
        }
        return copy;
    }

    private static bool IsSerializable(Type type)
    {
        while (type != typeof(object))
        {
            if ((type.Attributes & TypeAttributes.Serializable) != 0) return true;
            type = type.BaseType;
        }
        return false;
    }

    public static Vector2 Rotate(Vector2 v, float degrees)
    {
        float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

        float tx = v.x;
        float ty = v.y;
        v.x = (cos * tx) - (sin * ty);
        v.y = (sin * tx) + (cos * ty);
        return v;
    }
}