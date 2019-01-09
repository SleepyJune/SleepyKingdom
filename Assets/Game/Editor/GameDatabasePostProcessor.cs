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

    class AssetInfo
    {
        public string[] importedAssets;
        public string[] deletedAssets;
        public string[] movedAssets;
        public string[] movedFromAssetPaths;
    }

    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        if (database == null)
        {
            database = (GameDatabase)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/GameDataObjects/GameDatabase.asset", typeof(GameDatabase));
        }

        var info = new AssetInfo()
        {
            importedAssets = importedAssets,
            deletedAssets = deletedAssets,
            movedAssets = movedAssets,
            movedFromAssetPaths = movedFromAssetPaths,
        };

        CheckModified("/Prefabs/GameDataObjects/", ref database.allObjects, "*.asset", info);

        CheckModified("/Prefabs/Interactables/", ref database.allPrefabs, "*.prefab", info);

        /*if (mapDatabase == null)
        {
            mapDatabase = (MapDatabase)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/MapDataObjects/MapDatabase.asset", typeof(MapDatabase));
        }

        CheckModified("/Prefabs/MapDataObjects/", ref mapDatabase.allObjects, importedAssets, deletedAssets);*/
    }

    static void CheckModified<T>(string path, ref T[] collection, string searchPattern, AssetInfo info)
    {
        bool databaseModified = false;

        foreach (string str in info.importedAssets)
        {
            //Debug.Log("Reimported Asset: " + str);

            if (str.StartsWith("Assets" + path))
            {
                databaseModified = true;
                break;
            }

        }

        if (!databaseModified)
        {
            foreach (string str in info.deletedAssets)
            {
                //Debug.Log("Deleted Asset: " + str);

                if (str.StartsWith("Assets" + path))
                {
                    databaseModified = true;
                    break;
                }

            }
        }

        if (databaseModified)
        {
            EditorHelperFunctions.GenerateFromAsset(path, ref collection, database, searchPattern);
            CheckCollection(database.allObjects, info);
        }
    }

    static void CheckCollection(GameDataObject[] collection, AssetInfo info)
    {
        int counter = collection.Max(i => i.id) + 1;               

        HashSet<int> itemID = new HashSet<int>();
        HashSet<GameDataObject> checkItems = new HashSet<GameDataObject>();

        for(int i = collection.Length - 1; i >= 0; i--)
        {
            var item = collection[i];

            var path = AssetDatabase.GetAssetPath(item);

            if (info.importedAssets.Contains(path))
            {
                checkItems.Add(item);
            }
        }

        foreach (var item in collection)
        {
            foreach(var checkItem in checkItems)
            {
                if(item.id == checkItem.id)
                {
                    var path = AssetDatabase.GetAssetPath(item);
                    var path2 = AssetDatabase.GetAssetPath(checkItem);

                    if(path != path2)
                    {
                        checkItem.id = counter;

                        counter += 1;

                        EditorUtility.SetDirty(checkItem);

                        Debug.Log("Changed id: " + checkItem.name);
                    }
                }
            }

            if (itemID.Contains(item.id))
            {
                Debug.Log("GameDataObject with the same id: " + item.name);                
            }
            else
            {
                itemID.Add(item.id);
            }
        }
    }

}