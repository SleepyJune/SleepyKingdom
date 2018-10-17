using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public static class HexVectorExtensions
{
    //col = x
    //row = y

    public static int[][,] hexDirections = new int[][,] {
            new int[,]{{+1, 0}, {0, -1}, {-1, -1},{-1, 0}, {-1, +1}, {0, +1}},
            new int[,]{{+1,  0}, {+1, -1}, { 0, -1},{-1,  0}, { 0, +1}, {+1, +1}}
        };

    public static Vector3Int OffsetToCube(this Vector3Int hex)
    {
        var x = hex.x - (hex.y - (hex.y & 1)) / 2;
        var z = hex.y;
        var y = -x - z;
        return new Vector3Int(x, y, z);
    }

    public static double CubeDistance(this Vector3Int a, Vector3Int b)
    {
        return Math.Max(Math.Max(Math.Abs(a.x - b.x), Math.Abs(a.y - b.y)), Math.Abs(a.z - b.z));
    }

    public static double OffsetDistance(this Vector3Int a, Vector3Int b)
    {
        var ac = OffsetToCube(a);
        var bc = OffsetToCube(b);
        return CubeDistance(ac, bc);
    }
}