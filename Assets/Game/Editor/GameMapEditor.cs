using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Tilemaps;

using UnityEditor;

public class GameMapEditor : EditorWindow
{
    static GameDatabase database;
    Tilemap spawnMap;
    CastleSpawnTile selectedCastle;

    Vector3Int lastClickedTilePosition;

    [MenuItem("Game/Map Editor")]
    static void Init()
    {
        database = (GameDatabase)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/GameDataObjects/GameDatabase.asset", typeof(GameDatabase));

        EditorWindow.GetWindow(typeof(GameMapEditor)).Show();        
    }

    private void OnEnable()
    {
        SceneView.onSceneGUIDelegate += OnSceneGUI;

        var map = GameObject.Find("SpawnMap");
        if (map != null)
        {
            spawnMap = map.transform.Find("Spawn").GetComponent<Tilemap>();            
        }
    }

    private void OnDisable()
    {
        SceneView.onSceneGUIDelegate -= OnSceneGUI;
    }

    void OnGUI()
    {
        ShowCountryEditor();
    }

    void OnSceneGUI(SceneView sceneView)
    {
        if (Event.current.type == EventType.MouseDown)
        {
            //Debug.Log("TEST");
            /*if(GridSelection.active && GridSelection.target.name == "Spawn")
            {
                Debug.Log(GridSelection.position);
            }*/

            var tile = GetMousePositionTile<CastleSpawnTile>();

            if(tile != selectedCastle)
            {
                selectedCastle = tile;
                Repaint();
            }

        }
    }

    T GetMousePositionTile<T>() where T : TileBase
    {
        if (spawnMap != null)
        {
            Vector3 mousePos = Event.current.mousePosition;
            float ppp = EditorGUIUtility.pixelsPerPoint;
            mousePos.y = SceneView.currentDrawingSceneView.camera.pixelHeight - mousePos.y * ppp;
            mousePos.x *= ppp;

            Vector3 worldPos = SceneView.currentDrawingSceneView.camera.ScreenToWorldPoint(mousePos);
            
            var cellPos = spawnMap.WorldToCell(worldPos);

            lastClickedTilePosition = cellPos;

            var tile = spawnMap.GetTile<T>(cellPos);

            if (tile != null)
            {
                return tile;
            }
        }

        return null;
    }

    void ShowCountryEditor()
    {
        ShowCountry();


    }

    void ShowCountry()
    {
        if(selectedCastle != null)
        {
            if(selectedCastle.countryData == null)
            {
                EditorGUILayout.LabelField("Country Data not found.", EditorStyles.boldLabel);
                return;
            }

            EditorGUILayout.LabelField("Name: " + selectedCastle.countryData.country.countryName, EditorStyles.boldLabel);

        }
        else
        {

            if (GUILayout.Button("Create Castle"))
            {
            }

            if (GUILayout.Button("Create Animal"))
            {
            }


            if (GUILayout.Button("Create Sparklies"))
            {
            }
        }
    }

    void ShowCountryList()
    {

    }

}
