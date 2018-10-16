using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Tilemaps;

using Random = UnityEngine.Random;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class CastleTile : Tile
{

#if UNITY_EDITOR

    [MenuItem("Assets/Create/Game/Tiles/Castle Tile")]
    public static void CreateTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Castle Tile", "New Castle Tile", "Asset", "Save Castle Tile", "Assets");
        if (path == "")
            return;
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<CastleTile>(), path);
    }

#endif
}