using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

using UnityEngine;
using UnityEditor;

using System.Linq;

class EventGeneratorPostProcessor : AssetPostprocessor
{
    static EventDatabase database;

    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        if (database == null)
        {
            database = (EventDatabase)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Events/EventDatabase.asset", typeof(EventDatabase));
        }

        bool databaseModified = false;

        foreach (string str in importedAssets)
        {
            if (str.StartsWith("Assets/Prefabs/Events/"))
            {
                databaseModified = true;
                break;
            }

            //Debug.Log("Reimported Asset: " + str);            
        }

        foreach (string str in deletedAssets)
        {
            if (str.StartsWith("Assets/Prefabs/Events/"))
            {
                databaseModified = true;
                break;
            }

            //Debug.Log("Deleted Asset: " + str);
        }

        if (databaseModified)
        {
            EditorHelperFunctions.GenerateFromAsset("/Prefabs/Events", ref database.allEvents, database, "*.asset");                     
        }
    }
}