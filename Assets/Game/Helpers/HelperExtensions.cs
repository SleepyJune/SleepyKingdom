using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public static class HelperExtensions
{
    public static T GetObject<T>(this Dictionary<int, T> dictionary, int id)
    {
        T ret;
        if(dictionary.TryGetValue(id, out ret))
        {
            return ret;
        }

        return default(T);
    }
}
