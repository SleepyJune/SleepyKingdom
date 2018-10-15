using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Tilemaps;

using Random = UnityEngine.Random;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class GroundTile : Tile
{
    public Sprite[] sprites;

    public override void GetTileData(Vector3Int location, ITilemap tilemap, ref TileData tileData)
    {        
        int index = Random.Range(0, sprites.Length);
        if (index >= 0 && index < sprites.Length)
        {
            tileData.sprite = sprites[index];
        }
        else
        {
            Debug.LogWarning("Not enough sprites in RoadTile instance");
        }
    }

#if UNITY_EDITOR

    [MenuItem("Assets/Create/Game/Tiles/Ground Tile")]
    public static void CreateTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Ground Tile", "New Road Tile", "Asset", "Save Ground Tile", "Assets");
        if (path == "")
            return;
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<GroundTile>(), path);
    }

#endif
}