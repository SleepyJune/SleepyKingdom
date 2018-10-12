using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

using UnityEngine;
using UnityEditor;

using System.Linq;

class GameDatabasePostProcessor : AssetPostprocessor
{
    static GameDatabase database;

    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        if (database == null)
        {
            database = (GameDatabase)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Managers/GameDatabase.asset", typeof(GameDatabase));
        }

        CheckModified("/Prefabs/SpriteObjects/", ref database.allSprites, importedAssets, deletedAssets);
        CheckModified("/Prefabs/BuildingObjects/", ref database.allBuildings, importedAssets, deletedAssets);
    }

    static void CheckModified<T>(string path, ref T[] collection, string[] importedAssets, string[] deletedAssets)
    {
        bool databaseModified = false;

        foreach (string str in importedAssets)
        {
            if (str.StartsWith("Assets" + path))
            {
                databaseModified = true;
                break;
            }

            //Debug.Log("Reimported Asset: " + str);            
        }

        foreach (string str in deletedAssets)
        {
            if (str.StartsWith("Assets" + path))
            {
                databaseModified = true;
                break;
            }

            //Debug.Log("Deleted Asset: " + str);
        }

        if (databaseModified)
        {
            EditorHelperFunctions.GenerateFromAsset(path, ref collection, database, "*.asset");
        }
    }
}