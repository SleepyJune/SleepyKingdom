using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Tilemaps;

using Random = UnityEngine.Random;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class GroundTile : GameTileBase
{
    public bool isBlocked;

    public Sprite[] sprites;
    
    public override void GetTileData(Vector3Int location, ITilemap tilemap, ref TileData tileData)
    {
        tileData.sprite = this.sprite;
        tileData.color = this.color;
        tileData.transform = this.transform;
        tileData.gameObject = this.gameObject;
        tileData.flags = this.flags;

        int index = Random.Range(0, sprites.Length);
        if (index >= 0 && index < sprites.Length)
        {
            tileData.sprite = sprites[index];
        }
        else
        {
            Debug.LogWarning("Not enough sprites");
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