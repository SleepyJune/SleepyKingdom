using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

using UnityEngine;
using UnityEditor;


//[CustomEditor(typeof(CastleSpawnTile))]
public class CastleSpawnTileEditor : Editor
{
    CastleSpawnTile tile;

    GameDatabase database;

    private void OnEnable()
    {
        tile = target as CastleSpawnTile;

        database = (GameDatabase)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/GameDataObjects/GameDatabase.asset", typeof(GameDatabase));
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUI.BeginChangeCheck();

        if (GUILayout.Button("Save"))
        {

        }

        DrawDefaultInspector();

        if (EditorGUI.EndChangeCheck())
        {
            OnChangeCountryData();
        }

        /*var changedCountryData = EditorGUILayout.ObjectField(tile.countryData, typeof(CountryDataObject), true) as CountryDataObject;
        if(changedCountryData != tile.countryData)
        {
            OnChangeCountryData();

            tile.countryData = changedCountryData;
        }*/            

        serializedObject.ApplyModifiedProperties();
    }

    void OnChangeCountryData()
    {
        Debug.Log("validate");

        if(tile.countryData != null && tile.countryData.country.castlePrefabId != 0)
        {
            var castlePrefab = database.allPrefabs.FirstOrDefault(prefab => prefab.id == tile.countryData.country.castlePrefabId) as MapCastleUnit;
            if(castlePrefab != null)
            {
                tile.sprite = castlePrefab.image;
            }
        }
        else
        {
            tile.sprite = null;
        }
    }
}