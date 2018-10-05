using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

using UnityEngine;
using UnityEditor;

using System.Linq;

class SpriteObjectPostProcessor : AssetPostprocessor
{
    static SpriteDatabase database;

    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        if (database == null)
        {
            database = (SpriteDatabase)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/SpriteObjects/SpriteDatabase.asset", typeof(SpriteDatabase));
        }

        bool databaseModified = false;

        foreach (string str in importedAssets)
        {
            if (str.StartsWith("Assets/Prefabs/SpriteObjects/"))
            {
                databaseModified = true;
                break;
            }

            //Debug.Log("Reimported Asset: " + str);            
        }

        foreach (string str in deletedAssets)
        {
            if (str.StartsWith("Assets/Prefabs/SpriteObjects/"))
            {
                databaseModified = true;
                break;
            }

            //Debug.Log("Deleted Asset: " + str);
        }

        if (databaseModified)
        {
            EditorHelperFunctions.GenerateFromAsset("/Prefabs/SpriteObjects", ref database.allSprites, database, "*.asset");                     
        }
    }
}