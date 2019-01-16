using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[Serializable]
public class Territory
{
    public Vector3Int[] points = new Vector3Int[0];

    public HashSet<Vector3Int> pointsHashset = new HashSet<Vector3Int>();

    public void InitDictionary()
    {
        MakeHashset();
    }

    void MakeHashset()
    {
        HashSet<Vector3Int> ret = new HashSet<Vector3Int>();
        foreach (var point in points)
        {
            ret.Add(point);
        }

        pointsHashset = ret;
    }

    public void Save()
    {
        points = pointsHashset.ToArray();
    }
}
