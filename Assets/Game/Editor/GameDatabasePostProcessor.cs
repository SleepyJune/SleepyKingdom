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

    static MapDatabase mapDatabase;

    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        if (database == null)
        {
            database = (GameDatabase)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/GameDataObjects/GameDatabase.asset", typeof(GameDatabase));
        }

        CheckModified("/Prefabs/GameDataObjects/", ref database.allObjects, importedAssets, deletedAssets);

        if (mapDatabase == null)
        {
            mapDatabase = (MapDatabase)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/MapDataObjects/MapDatabase.asset", typeof(MapDatabase));
        }

        CheckModified("/Prefabs/MapDataObjects/", ref mapDatabase.allObjects, importedAssets, deletedAssets);
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
            CheckCollection(database.allObjects);
            EditorHelperFunctions.GenerateFromAsset(path, ref collection, database, "*.asset");
        }
    }

    static void CheckCollection(GameDataObject[] collection)
    {
        int counter = collection.Max(i => i.id) + 1;               

        HashSet<int> itemID = new HashSet<int>();

        foreach (var item in collection)
        {
            if (itemID.Contains(item.id))
            {
                item.id = counter;

                //Debug.Log("GameDataObject with the same id: " + item.name);

                counter += 1;

                EditorUtility.SetDirty(item);
            }
            else
            {
                itemID.Add(item.id);
            }
        }
    }

}