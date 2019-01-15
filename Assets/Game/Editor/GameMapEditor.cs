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
        Territory,
    }

    GameDatabase database;
    MapDatabase mapDatabase;

    TerritoryTile territoryTile;

    Tilemap spawnMap;
    Tilemap territoryMap;

    SpawnTile selectedTile;
    SpawnTile previousSelectedTile;
    CountryDataObject selectedCountry;

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

        territoryTile = (TerritoryTile)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Tiles/TerritoryTiles/TerritoryTile.asset", typeof(TerritoryTile));

        if (territoryTile == null)
        {
            Debug.Log("Cannot find TerritoryTile");
        }

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

        map = GameObject.Find("TerritoryMap");
        if (map != null)
        {
            territoryMap = map.transform.Find("Territory").GetComponent<Tilemap>();
        }
        else
        {
            Debug.Log("Cannot find TerritoryMap");
        }

        selectedCountry = null;
        selectedCreateType = MapDataType.None;
        ShowTerritory(false);

        Tools.current = Tool.None;
        Tools.hidden = true;
    }

    private void OnDisable()
    {
        SceneView.onSceneGUIDelegate -= OnSceneGUI;

        Tools.current = Tool.Move;
        Tools.hidden = false;
    }

    private void InitTextures()
    {
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

    private void RefreshMap()
    {
        if (spawnMap != null)
        {
            spawnMap.ClearAllTiles();
            
            foreach (var spawnTile in mapDatabase.castleSpawnTiles)
            {
                spawnMap.SetTile(spawnTile.position, spawnTile);
            }

            foreach (var spawnTile in mapDatabase.interactableSpawnTiles)
            {
                spawnMap.SetTile(spawnTile.position, spawnTile);
            }

            EditorUtility.SetDirty(spawnMap);
        }
    }

    private void RefreshDatabase()
    {
        InitTextures();
    }

    void OnGUI()
    {
        EditorGUILayout.Vector3IntField("Position: ", lastClickedTilePosition);
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        if (selectedCountry != null)
        {
            if (selectedCreateType == MapDataType.Territory)
            {
                ShowEditTerritoryMenu();
                return;
            }
        }

        if(!(selectedTile is CastleSpawnTile))
        {
            ShowTerritory(false);
        }

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

        GUILayout.Space(25);

        if (GUILayout.Button("Refresh Database"))
        {
            RefreshDatabase();
        }

        if (GUILayout.Button("Refresh Map"))
        {
            RefreshMap();
        }
    }

    void OnSceneGUI(SceneView sceneView)
    {
        HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));

        if (Event.current.button == 0)
        {
            if (Event.current.type == EventType.MouseDown)
            {
                var tile = GetMousePositionTile<SpawnTile>();

                if (tile != selectedTile)
                {
                    if (tile is CastleSpawnTile)
                    {
                        SetNewCastle(tile as CastleSpawnTile);
                    }
                    else if (tile is InteractableSpawnTile)
                    {
                        SetNewInteractable(tile as InteractableSpawnTile);
                    }
                }

                selectedTile = tile;
                Repaint();
            }
            else if(Event.current.type == EventType.MouseDrag)
            {
                SetTerritoryTile(GetMousePosition());
                Repaint();
            }            
        }
    }

    Vector3Int GetMousePosition()
    {
        if (spawnMap != null)
        {
            Vector3 mousePos = Event.current.mousePosition;
            float ppp = EditorGUIUtility.pixelsPerPoint;
            mousePos.y = SceneView.currentDrawingSceneView.camera.pixelHeight - mousePos.y * ppp;
            mousePos.x *= ppp;

            Vector3 worldPos = SceneView.currentDrawingSceneView.camera.ScreenToWorldPoint(mousePos);

            return spawnMap.WorldToCell(worldPos);
        }

        return new Vector3Int(0, 0, 0);
    }

    T GetMousePositionTile<T>() where T : TileBase
    {
        if (spawnMap != null)
        {
            var cellPos = GetMousePosition();

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

            EditorUtility.SetDirty(mapDatabase);
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

            EditorUtility.SetDirty(mapDatabase);
        }

        GUILayout.EndScrollView();
    }

    bool removeCountryConfirmation = false;
    bool isShowingTerritory = false;
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

            if(selectedCountry != selectedCastle.countryData)
            {
                selectedCountry = selectedCastle.countryData;
            }

            EditorGUILayout.LabelField("Name: " + selectedCastle.countryData.country.countryName, EditorStyles.boldLabel);
            EditorGUILayout.Separator();

            if(subEditor != null)
            {
                subEditor.DrawDefaultInspector();
            }

            isShowingTerritory = EditorGUILayout.Toggle("Show Territory", isShowingTerritory);
            ShowTerritory(isShowingTerritory);

            if (GUILayout.Button("Edit Territory"))
            {
                previousSelectedTile = selectedTile;
                selectedCreateType = MapDataType.Territory;
                ShowTerritory(true);
            }

            if (!removeCountryConfirmation && GUILayout.Button("Remove"))
            {
                removeCountryConfirmation = true;
            }

            if (removeCountryConfirmation)
            {
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Confirm"))
                {
                    removeCountryConfirmation = false;

                    spawnMap.SetTile(selectedCastle.position, null);

                    mapDatabase.countryDataObjectDictionary.Remove(selectedCastle.countryData.country.countryID);
                    mapDatabase.castleSpawnTileDictionary.Remove(selectedCastle.position);

                    mapDatabase.Save();

                    DestroyImmediate(selectedCastle.countryData, true);
                    DestroyImmediate(selectedCastle, true);
                }

                if (GUILayout.Button("Cancel"))
                {
                    removeCountryConfirmation = false;
                }
                GUILayout.EndHorizontal();
            }
        }
    }

    void ShowEditTerritoryMenu()
    {
        if (GUILayout.Button("Back"))
        {
            selectedCountry = null;
            selectedCreateType = MapDataType.None;

            ShowTerritory(false);

            selectedTile = previousSelectedTile;
        }

        if (selectedCountry == null)
        {
            selectedCreateType = MapDataType.None;
            return;
        }

        string[] selectionString = {"Add","Delete"};
                
        EditorGUILayout.Space();
        selectionGridIndex = GUILayout.SelectionGrid(selectionGridIndex, selectionString, 2, GUI.skin.button, GUILayout.Height(50));

    }

    void SetTerritoryTile(Vector3Int position)
    {
        if(selectedCreateType == MapDataType.Territory && selectedCountry != null)
        {
            if(selectionGridIndex == 0)
            {
                selectedCountry.country.territory.pointsHashset.Add(position);
                territoryMap.SetTile(position, territoryTile);
            }
            else
            {
                selectedCountry.country.territory.pointsHashset.Remove(position);
                territoryMap.SetTile(position, null);
            }

            selectedCountry.country.territory.Save();
            
            EditorUtility.SetDirty(selectedCountry);
        }
    }

    CountryDataObject lastShownTerritoryCountry;
    void ShowTerritory(bool show)
    {
        if (show)
        {
            if (selectedCountry != null)
            {
                if(lastShownTerritoryCountry != selectedCountry)
                {                    
                    DrawTerritoryMap();
                }
            }
        }
        else
        {
            if (territoryMap.gameObject.activeInHierarchy)
            {
                lastShownTerritoryCountry = null;
                territoryMap.gameObject.SetActive(false);
            }
        }
    }

    void DrawTerritoryMap()
    {
        if (selectedCountry != null)
        {
            if(selectedCountry.country.territory == null)
            {
                Debug.Log("Territory not found");
                return;
            }

            territoryMap.ClearAllTiles();

            selectedCountry.country.territory.InitDictionary();

            foreach(var point in selectedCountry.country.territory.points)
            {
                territoryMap.SetTile(point, territoryTile);
            }

            lastShownTerritoryCountry = selectedCountry;
            territoryMap.gameObject.SetActive(true);
        }
    }

}
