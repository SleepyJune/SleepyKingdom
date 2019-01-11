using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Tilemaps;

using UnityEditor;


public class GameMapEditor : EditorWindow
{
    enum MapDataType
    {
        None,
        Country,
        Animal,
        Interactable,
    }

    GameDatabase database;
    MapDatabase mapDatabase;
        
    Tilemap spawnMap;
    SpawnTile selectedTile;

    Editor subEditor;

    MapDataType selectedCreateType = MapDataType.None;

    Vector3Int lastClickedTilePosition;

    Vector2 scrollPos;

    MapCastleUnit[] castlePrefabs = new MapCastleUnit[0];
    Texture[] castleTextures = new Texture[0];

    MapInteractableUnit[] interactables = new MapInteractableUnit[0];
    Texture[] interactableTextures = new Texture[0];

    int selectionGridIndex = 0;

    [MenuItem("Game/Map Editor")]
    static void Init()
    {
        EditorWindow.GetWindow(typeof(GameMapEditor)).Show();
    }

    private void OnEnable()
    {
        database = (GameDatabase)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/GameDataObjects/GameDatabase.asset", typeof(GameDatabase));
        mapDatabase = (MapDatabase)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/MapDataObjects/MapDatabase.asset", typeof(MapDatabase));

        if (database == null)
        {
            Debug.Log("Cannot find GameDatabase");
        }

        if(mapDatabase == null)
        {
            Debug.Log("Cannot find MapDatabase");
        }
        else
        {
            mapDatabase.InitDictionary();
        }

        InitTextures();

        SceneView.onSceneGUIDelegate += OnSceneGUI;

        var map = GameObject.Find("SpawnMap");
        if (map != null)
        {
            spawnMap = map.transform.Find("Spawn").GetComponent<Tilemap>();
        }
        else
        {
            Debug.Log("Cannot find SpawnMap");
        }
    }

    private void OnDisable()
    {
        SceneView.onSceneGUIDelegate -= OnSceneGUI;
    }

    private void InitTextures()
    {
        foreach (var obj in database.allObjects)
        {
                       
        }

        List<Texture> interactableTextureList = new List<Texture>();
        List<MapInteractableUnit> interactableList = new List<MapInteractableUnit>();

        List<Texture> castleTexturesList = new List<Texture>();
        List<MapCastleUnit> castleObjectsList = new List<MapCastleUnit>();

        foreach (var obj in database.allPrefabs)
        {
            var interactable = obj as MapInteractableUnit;
            if (interactable != null)
            {
                var texture = AssetPreview.GetAssetPreview(interactable.image);

                interactableTextureList.Add(texture);
                interactableList.Add(interactable);
            }            

            var castleObject = obj as MapCastleUnit;
            if (castleObject != null)
            {
                var texture = AssetPreview.GetAssetPreview(castleObject.image);

                castleTexturesList.Add(texture);
                castleObjectsList.Add(castleObject);
            }
        }

        interactableTextures = interactableTextureList.ToArray();
        interactables = interactableList.ToArray();

        castleTextures = castleTexturesList.ToArray();
        castlePrefabs = castleObjectsList.ToArray();
    }

    private void RefreshDatabase()
    {
        InitTextures();
    }

    void OnGUI()
    {
        EditorGUILayout.Vector3IntField("Position: ", lastClickedTilePosition);
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        if(selectedTile != null)
        {
            if(selectedTile is CastleSpawnTile)
            {
                ShowCountry();
            }
            else if(selectedTile is InteractableSpawnTile)
            {
                ShowInteractable();
            }

            return;
        }

        if (selectedCreateType != MapDataType.None)
        {
            if (GUILayout.Button("Back"))
            {
                selectedCreateType = MapDataType.None;
            }

            if (selectedCreateType == MapDataType.Country)
            {
                ShowCreateCastleMenu();
            }
            else if (selectedCreateType == MapDataType.Interactable)
            {
                ShowCreateInteractableMenu();
            }

            return;
        }

        if (GUILayout.Button("Create Castle"))
        {
            selectionGridIndex = 0;
            selectedCreateType = MapDataType.Country;
        }

        if (GUILayout.Button("Create Animal"))
        {
            selectionGridIndex = 0;
            selectedCreateType = MapDataType.Animal;
        }

        if (GUILayout.Button("Create Interactable"))
        {
            selectionGridIndex = 0;
            selectedCreateType = MapDataType.Interactable;
        }

        if (GUILayout.Button("Refresh Database"))
        {
            RefreshDatabase();
        }
    }

    void OnSceneGUI(SceneView sceneView)
    {
        if (Event.current.type == EventType.MouseDown)
        {
            var tile = GetMousePositionTile<SpawnTile>();
                        
            if (tile != selectedTile)
            {
                if(tile is CastleSpawnTile)
                {
                    SetNewCastle(tile as CastleSpawnTile);
                }
                else if(tile is InteractableSpawnTile)
                {
                    SetNewInteractable(tile as InteractableSpawnTile);
                }
            }

            selectedTile = tile;

            Repaint();
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

    void SetNewCastle(CastleSpawnTile newTile)
    {
        selectedTile = newTile;

        if(subEditor != null)
        {
            DestroyImmediate(subEditor);
        }

        if(newTile != null && newTile.countryData != null && newTile.countryData.country != null)
        {
            subEditor = Editor.CreateEditor(newTile.countryData);
        }
    }

    void SetNewInteractable(InteractableSpawnTile newTile)
    {
        selectedTile = newTile;
    }

    void ShowCreateInteractableMenu()
    {
        if (mapDatabase.interactableSpawnTileDictionary.ContainsKey(lastClickedTilePosition))
        {
            EditorGUILayout.LabelField("Tile already contains an interactable.", EditorStyles.boldLabel);
            return;
        }

        scrollPos = GUILayout.BeginScrollView(scrollPos, false, true);

        int columns = (int)Math.Floor(Screen.width / 256.0f);

        selectionGridIndex = GUILayout.SelectionGrid(selectionGridIndex, interactableTextures, columns, GUI.skin.button);

        EditorGUILayout.Space();

        if (GUILayout.Button("Add"))
        {
            var interactable = interactables[selectionGridIndex];

            var newTile = CreateInstance(typeof(InteractableSpawnTile)) as InteractableSpawnTile;
            newTile.name = "Interactable SpawnTile";
            newTile.prefab = interactable;
            newTile.position = lastClickedTilePosition;
            newTile.sprite = interactable.image;
            newTile.flags = TileFlags.LockTransform;

            var matrix = Matrix4x4.Scale(new Vector3(.5f, .5f, 1f));
            matrix.m33 = 0;
            newTile.transform = matrix;

            mapDatabase.interactableSpawnTileDictionary.Add(newTile.position, newTile);

            AssetDatabase.AddObjectToAsset(newTile, mapDatabase);

            spawnMap.SetTile(lastClickedTilePosition, newTile);

            mapDatabase.Save();

            SetNewInteractable(newTile);
        }
        GUILayout.EndScrollView();
    }

    void ShowInteractable()
    {
        var selectedInteractable = selectedTile as InteractableSpawnTile;

        if (selectedInteractable != null)
        {
            EditorGUILayout.LabelField("Interactable", EditorStyles.boldLabel);
            EditorGUILayout.Separator();

            /*if (subEditor != null)
            {
                subEditor.DrawDefaultInspector();
            }*/

            if (GUILayout.Button("Remove"))
            {
                spawnMap.SetTile(selectedInteractable.position, null);
                                
                mapDatabase.interactableSpawnTileDictionary.Remove(selectedInteractable.position);

                mapDatabase.Save();

                DestroyImmediate(selectedInteractable, true);
            }
        }
    }

    void ShowCreateCastleMenu()
    {
        if (mapDatabase.castleSpawnTileDictionary.ContainsKey(lastClickedTilePosition))
        {
            EditorGUILayout.LabelField("Tile already contains a castle.", EditorStyles.boldLabel);
            return;
        }

        scrollPos = GUILayout.BeginScrollView(scrollPos, false, true);

        int columns = (int)Math.Floor(Screen.width / 256.0f);

        selectionGridIndex = GUILayout.SelectionGrid(selectionGridIndex, castleTextures, columns, GUI.skin.button);

        EditorGUILayout.Space();

        if (GUILayout.Button("Add"))
        {
            var castleObject = castlePrefabs[selectionGridIndex];

            var newCountryData = CreateInstance(typeof(CountryDataObject)) as CountryDataObject;
            var newCountry = new Country();
            newCountryData.country = newCountry;
            newCountryData.country.countryID = mapDatabase.countryCounter;
            newCountryData.country.countryName = "Country " + newCountryData.country.countryID;
            newCountryData.country.castlePrefabId = castleObject.id;
            newCountryData.country.position = lastClickedTilePosition;
            newCountryData.name = newCountry.countryName + " Data";

            mapDatabase.countryDataObjectDictionary.Add(mapDatabase.countryCounter, newCountryData);
            mapDatabase.countryCounter += 1;

            var newTile = CreateInstance(typeof(CastleSpawnTile)) as CastleSpawnTile;
            newTile.name = newCountry.countryName + " SpawnTile";
            newTile.countryData = newCountryData;
            newTile.position = lastClickedTilePosition;
            newTile.sprite = castleObject.image;

            mapDatabase.castleSpawnTileDictionary.Add(newTile.position, newTile);

            AssetDatabase.AddObjectToAsset(newTile, mapDatabase);
            AssetDatabase.AddObjectToAsset(newCountryData, mapDatabase);

            spawnMap.SetTile(lastClickedTilePosition, newTile);

            mapDatabase.Save();

            SetNewCastle(newTile);
        }

        GUILayout.EndScrollView();
    }

    void ShowCountry()
    {
        var selectedCastle = selectedTile as CastleSpawnTile;

        if (selectedCastle != null)
        {
            if(selectedCastle.countryData == null)
            {
                EditorGUILayout.LabelField("Country Data not found.", EditorStyles.boldLabel);
                return;
            }

            EditorGUILayout.LabelField("Name: " + selectedCastle.countryData.country.countryName, EditorStyles.boldLabel);
            EditorGUILayout.Separator();

            if(subEditor != null)
            {
                subEditor.DrawDefaultInspector();
            }

            if (GUILayout.Button("Remove"))
            {
                spawnMap.SetTile(selectedCastle.position, null);

                mapDatabase.countryDataObjectDictionary.Remove(selectedCastle.countryData.country.countryID);
                mapDatabase.castleSpawnTileDictionary.Remove(selectedCastle.position);

                mapDatabase.Save();

                DestroyImmediate(selectedCastle.countryData, true);
                DestroyImmediate(selectedCastle, true);
            }
        }
    }

}
