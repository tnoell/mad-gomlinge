using System;
using System.Collections.Generic;
using UnityEngine;

enum Direction
{
    up = 0,
    right = 1,
    down = 2,
    left = 3
}
class DirectionUtil
{

    public static Array AllDirections()
    {
        return Enum.GetValues(typeof(Direction));
    }
    public static Direction GetOpposite(Direction dir)
    {
        return (Direction)( ( (int)dir + 2) % 4);
    }

    public static Vector2Int GetVector2Int(Direction dir)
    {
        switch(dir)
        {
            case Direction.up: return Vector2Int.up;
            case Direction.right: return Vector2Int.right;
            case Direction.down: return Vector2Int.down;
            case Direction.left: return Vector2Int.left;
            default: throw new Exception("Unhandled direction");
        }
    }
}
