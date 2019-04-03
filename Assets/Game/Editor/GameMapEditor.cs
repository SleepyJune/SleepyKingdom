using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

using UnityEditor;
using UnityEditor.SceneManagement;


public class GameMapEditor : EditorWindow
{
    enum MapDataType
    {
        None,
        Country,
        Animal,
        Interactable,
        Territory,
        Map,
    }

    GameDatabase database;
    MapDatabase mapDatabase;

    MapDatabaseObject currentMap;
    
    Tilemap spawnMap;

    Transform tilemapParent;

    SpawnTile selectedTile;
    SpawnTile previousSelectedTile;
    CountryDataObject selectedCountry;

    Editor subEditor;

    MapDataType selectedCreateType = MapDataType.None;

    Vector3Int lastClickedTilePosition;

    Vector2 scrollPos;

    MapCastleUnit[] castlePrefabs = new MapCastleUnit[0];
    GUIContent[] castleContents = new GUIContent[0];

    MapInteractableUnit[] interactables = new MapInteractableUnit[0];
    GUIContent[] interactableContents = new GUIContent[0];

    AnimalUnit[] animals = new AnimalUnit[0];
    GUIContent[] animalContents = new GUIContent[0];

    int selectionGridIndex = 0;

    bool initSuccessful = false;

    Scene currentScene;

    [MenuItem("Game/Map Editor")]
    static void Init()
    {
        EditorWindow.GetWindow(typeof(GameMapEditor)).Show();
    }

    private void OnEnable()
    {
        EditorSceneManager.activeSceneChangedInEditMode += ActiveSceneChanged;
        currentScene = SceneManager.GetActiveScene();

        Initialize();
    }

    private void OnDisable()
    {
        EditorSceneManager.activeSceneChangedInEditMode -= ActiveSceneChanged;
    }

    private void ActiveSceneChanged(Scene current, Scene next)
    {
        currentScene = next;

        if(currentScene.name != "MapScene")
        {
            CleanUpOnSceneChange();
        }
        else
        {
            Initialize();
        }
    }

    private void Initialize()
    {
        initSuccessful = true;

        database = (GameDatabase)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/GameDataObjects/GameDatabase.asset", typeof(GameDatabase));
        mapDatabase = (MapDatabase)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/MapDataObjects/MapDatabase.asset", typeof(MapDatabase));

        if (database == null)
        {
            initSuccessful = false;
            Debug.Log("Cannot find GameDatabase");
        }

        if (mapDatabase == null)
        {
            initSuccessful = false;
            Debug.Log("Cannot find MapDatabase");
        }
        else
        {
            mapDatabase.InitDictionary();
        }

        InitTextures();

        SceneView.onSceneGUIDelegate += OnSceneGUI;

        var terrain = GameObject.Find("TerrainMap");
        if (terrain)
        {
            tilemapParent = terrain.transform;
        }

        var map = GameObject.Find("SpawnMap");
        if (map != null)
        {
            spawnMap = map.transform.Find("Spawn").GetComponent<Tilemap>();
        }
        else
        {
            initSuccessful = false;
            Debug.Log("Cannot find SpawnMap");
        }

        selectedCountry = null;
        selectedCreateType = MapDataType.None;

        Tools.current = Tool.None;
        Tools.hidden = true;
    }

    private void CleanUpOnSceneChange()
    {
        SceneView.onSceneGUIDelegate -= OnSceneGUI;

        Tools.current = Tool.Move;
        Tools.hidden = false;
    }

    private void InitTextures()
    {
        List<GUIContent> interactableContentsList = new List<GUIContent>();
        List<MapInteractableUnit> interactableList = new List<MapInteractableUnit>();

        List<GUIContent> castleContentsList = new List<GUIContent>();
        List<MapCastleUnit> castleObjectsList = new List<MapCastleUnit>();

        List<GUIContent> animalContentsList = new List<GUIContent>();
        List<AnimalUnit> animalList = new List<AnimalUnit>();

        foreach (var obj in database.allPrefabs)
        {
            var interactable = obj as MapInteractableUnit;
            if (interactable != null)
            {
                var texture = AssetPreview.GetAssetPreview(interactable.image);

                GUIContent content = new GUIContent(texture, interactable.name);

                interactableContentsList.Add(content);
                interactableList.Add(interactable);
                continue;
            }            

            var castleObject = obj as MapCastleUnit;
            if (castleObject != null)
            {
                var texture = AssetPreview.GetAssetPreview(castleObject.image);

                GUIContent content = new GUIContent(texture, castleObject.name);

                castleContentsList.Add(content);
                castleObjectsList.Add(castleObject);
                continue;
            }

            var animal = obj as AnimalUnit;
            if (animal != null)
            {
                var texture = AssetPreview.GetAssetPreview(animal.faceImage);

                GUIContent content = new GUIContent(texture, animal.name);

                animalContentsList.Add(content);
                animalList.Add(animal);
                continue;
            }
        }

        animalContents = animalContentsList.ToArray();
        animals = animalList.ToArray();

        interactableContents = interactableContentsList.ToArray();
        interactables = interactableList.ToArray();

        castleContents = castleContentsList.ToArray();
        castlePrefabs = castleObjectsList.ToArray();
    }

    private void RefreshMap()
    {
        if (spawnMap != null)
        {
            spawnMap.ClearAllTiles();

            if (currentMap != null)
            {
                ChangeTilemap();

                List<Vector3Int> tilesToDelete = new List<Vector3Int>();

                foreach (var spawnTile in currentMap.castleSpawnTiles)
                {
                    spawnMap.SetTile(spawnTile.position, spawnTile);
                }

                foreach (var spawnTile in currentMap.interactableSpawnTiles)
                {
                    spawnMap.SetTile(spawnTile.position, spawnTile);
                }

                foreach (var spawnTile in currentMap.animalSpawnTiles)
                {
                    spawnMap.SetTile(spawnTile.position, spawnTile);
                }
            }

            EditorUtility.SetDirty(spawnMap);
        }
    }

    void ChangeTilemap()
    {
        if (tilemapParent != null)
        {
            foreach (Transform child in tilemapParent)
            {
                child.gameObject.SetActive(false);
            }

            var mapTransform = tilemapParent.Find(currentMap.name);
            if (mapTransform)
            {
                mapTransform.gameObject.SetActive(true);
            }
        }
    }

    private void RefreshDatabase()
    {
        mapDatabase.InitDictionary();
        InitTextures();
    }

    string newMapName = "";
    private void ShowMapSelectMenu()
    {
        EditorGUILayout.Space();

        if (selectedCreateType == MapDataType.Map)
        {
            EditorGUILayout.LabelField("Create New Map");

            newMapName = EditorGUILayout.TextField(newMapName);

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Create"))
            {
                var newMap = CreateInstance(typeof(MapDatabaseObject)) as MapDatabaseObject;
                newMap.name = newMapName;

                mapDatabase.mapDatabaseDictionary.Add(newMap.name, newMap);
                mapDatabase.Save();

                AssetDatabase.AddObjectToAsset(newMap, mapDatabase);
                EditorUtility.SetDirty(mapDatabase);

                currentMap = newMap;

                selectedCreateType = MapDataType.None;
            }

            if (GUILayout.Button("Cancel"))
            {
                selectedCreateType = MapDataType.None;
            }
            GUILayout.EndHorizontal();

            return;
        }

        if (GUILayout.Button("Create New Map"))
        {
            selectedCreateType = MapDataType.Map;
        }

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Map List");
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        foreach (var map in mapDatabase.mapDatabaseObjects.OrderBy(m=>m.name))
        {
            if (GUILayout.Button(map.name))
            {
                map.InitDictionary();
                currentMap = map;

                RefreshMap();
            }
        }        
    }

    bool removeMapConfirmation = false;
    void OnGUI()
    {
        if (!initSuccessful)
        {
            EditorGUILayout.LabelField("Initialization failed.", EditorStyles.boldLabel);
            return;
        }

        if (!Application.isEditor || Application.isPlaying)
        {
            EditorGUILayout.LabelField("GameMapEditor only works during edit mode.", EditorStyles.boldLabel);
            return;
        }

        if (currentScene.name != "MapScene")
        {
            EditorGUILayout.LabelField("GameMapEditor only works in MapScene.", EditorStyles.boldLabel);
            return;
        }

        if(currentMap == null)
        {
            ShowMapSelectMenu();
            return;
        }

        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(currentMap.name);

        if (GUILayout.Button("Map Menu"))
        {
            currentMap = null;

            RefreshMap();
        }
        GUILayout.EndHorizontal();

        EditorGUILayout.Vector3IntField("Position: ", lastClickedTilePosition);
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        if (selectedCountry != null)
        {

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
            else if(selectedTile is AnimalSpawnTile)
            {
                ShowAnimal();
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
            else if(selectedCreateType == MapDataType.Animal)
            {
                ShowCreateAnimalMenu();
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

        if (!removeMapConfirmation && GUILayout.Button("Remove Map"))
        {
            removeMapConfirmation = true;
        }

        if (removeMapConfirmation)
        {
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Confirm"))
            {
                removeMapConfirmation = false;

                mapDatabase.mapDatabaseDictionary.Remove(currentMap.name);
                mapDatabase.Save();

                DestroyImmediate(currentMap, true);

                EditorUtility.SetDirty(mapDatabase);

                RefreshMap();
            }

            if (GUILayout.Button("Cancel"))
            {
                removeMapConfirmation = false;
            }

            GUILayout.EndHorizontal();
        }
    }

    void OnSceneGUI(SceneView sceneView)
    {
        if (!Application.isEditor || Application.isPlaying || !initSuccessful || currentScene.name != "MapScene")
        {
            return;
        }

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
                        SetSelectedTile(tile as InteractableSpawnTile);
                    }
                }

                selectedTile = tile;
                Repaint();
            }
            else if(Event.current.type == EventType.MouseDrag)
            {

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

    void SetSelectedTile(SpawnTile newTile)
    {
        selectedTile = newTile;
    }

    void ShowCreateInteractableMenu()
    {
        if (currentMap.interactableSpawnTileDictionary.ContainsKey(lastClickedTilePosition))
        {
            EditorGUILayout.LabelField("Tile already contains an interactable.", EditorStyles.boldLabel);
            return;
        }

        scrollPos = GUILayout.BeginScrollView(scrollPos, false, true);

        int columns = (int)Math.Floor(Screen.width / 256.0f);

        selectionGridIndex = GUILayout.SelectionGrid(selectionGridIndex, interactableContents, columns, GUI.skin.button);

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

            currentMap.interactableSpawnTileDictionary.Add(newTile.position, newTile);

            AssetDatabase.AddObjectToAsset(newTile, currentMap);

            spawnMap.SetTile(lastClickedTilePosition, newTile);

            currentMap.Save();

            SetSelectedTile(newTile);

            EditorUtility.SetDirty(currentMap);
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

                currentMap.interactableSpawnTileDictionary.Remove(selectedInteractable.position);

                currentMap.Save();

                DestroyImmediate(selectedInteractable, true);
            }
        }
    }

    void ShowCreateAnimalMenu()
    {
        if (currentMap.animalSpawnTileDictionary.ContainsKey(lastClickedTilePosition))
        {
            EditorGUILayout.LabelField("Tile already contains an animal.", EditorStyles.boldLabel);
            return;
        }

        scrollPos = GUILayout.BeginScrollView(scrollPos, false, true);

        int columns = (int)Math.Floor(Screen.width / 256.0f);

        selectionGridIndex = GUILayout.SelectionGrid(selectionGridIndex, animalContents, columns, GUI.skin.button);

        EditorGUILayout.Space();

        if (GUILayout.Button("Add"))
        {
            var animal = animals[selectionGridIndex];

            var newTile = CreateInstance(typeof(AnimalSpawnTile)) as AnimalSpawnTile;
            newTile.name = "Animal SpawnTile";
            newTile.prefab = animal;
            newTile.position = lastClickedTilePosition;
            newTile.sprite = animal.faceImage;
            newTile.flags = TileFlags.LockTransform;

            var matrix = Matrix4x4.Scale(new Vector3(.5f, .5f, 1f));
            matrix.m33 = 0;
            newTile.transform = matrix;

            currentMap.animalSpawnTileDictionary.Add(newTile.position, newTile);

            AssetDatabase.AddObjectToAsset(newTile, currentMap);

            spawnMap.SetTile(lastClickedTilePosition, newTile);

            currentMap.Save();

            SetSelectedTile(newTile);

            EditorUtility.SetDirty(currentMap);
        }
        GUILayout.EndScrollView();
    }

    void ShowAnimal()
    {
        var selectedAnimal = selectedTile as AnimalSpawnTile;

        if (selectedAnimal != null)
        {
            EditorGUILayout.LabelField("Animal", EditorStyles.boldLabel);
            EditorGUILayout.Separator();

            if (GUILayout.Button("Remove"))
            {
                spawnMap.SetTile(selectedAnimal.position, null);

                currentMap.animalSpawnTileDictionary.Remove(selectedAnimal.position);

                currentMap.Save();

                DestroyImmediate(selectedAnimal, true);
            }
        }
    }

    void ShowCreateCastleMenu()
    {
        if (currentMap.castleSpawnTileDictionary.ContainsKey(lastClickedTilePosition))
        {
            EditorGUILayout.LabelField("Tile already contains a castle.", EditorStyles.boldLabel);
            return;
        }

        scrollPos = GUILayout.BeginScrollView(scrollPos, false, true);

        int columns = (int)Math.Floor(Screen.width / 256.0f);

        selectionGridIndex = GUILayout.SelectionGrid(selectionGridIndex, castleContents, columns, GUI.skin.button);

        EditorGUILayout.Space();

        if (GUILayout.Button("Add"))
        {
            var castleObject = castlePrefabs[selectionGridIndex];

            var newCountryData = CreateInstance(typeof(CountryDataObject)) as CountryDataObject;
            var newCountry = new Country();
            newCountryData.country = newCountry;
            newCountryData.country.countryID = currentMap.countryCounter;
            newCountryData.country.countryName = "Country " + newCountryData.country.countryID;
            newCountryData.country.castlePrefabId = castleObject.id;
            newCountryData.country.position = lastClickedTilePosition;
            newCountryData.name = newCountry.countryName + " Data";

            currentMap.countryDataObjectDictionary.Add(currentMap.countryCounter, newCountryData);
            currentMap.countryCounter += 1;

            var newTile = CreateInstance(typeof(CastleSpawnTile)) as CastleSpawnTile;
            newTile.name = newCountry.countryName + " SpawnTile";
            newTile.countryData = newCountryData;
            newTile.position = lastClickedTilePosition;
            newTile.sprite = castleObject.image;

            currentMap.castleSpawnTileDictionary.Add(newTile.position, newTile);

            AssetDatabase.AddObjectToAsset(newTile, currentMap);
            AssetDatabase.AddObjectToAsset(newCountryData, currentMap);

            spawnMap.SetTile(lastClickedTilePosition, newTile);

            currentMap.Save();

            SetNewCastle(newTile);

            EditorUtility.SetDirty(currentMap);
        }

        GUILayout.EndScrollView();
    }

    bool removeCountryConfirmation = false;
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

                    currentMap.countryDataObjectDictionary.Remove(selectedCastle.countryData.country.countryID);
                    currentMap.castleSpawnTileDictionary.Remove(selectedCastle.position);

                    currentMap.Save();

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
}
