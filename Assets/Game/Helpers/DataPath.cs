using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class DataPath
{
    public static string savePath
    {
        get
        {
            if (Application.platform == RuntimePlatform.Android
             || Application.platform == RuntimePlatform.WebGLPlayer)
            {
                return Application.persistentDataPath + "/Saves/";
            }
            else
            {
                return Application.dataPath + "/Saves/";
            }
        }
    }
}