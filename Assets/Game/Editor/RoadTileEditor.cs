using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

using UnityEditor;
using UnityEditor.SceneManagement;


public class RoadTileEditor : EditorWindow
{
    bool initSuccessful = false;
    Scene currentScene;

    RoadTile selectedTile;
    Vector3Int selectedTilePos;
    Vector3Int lastClickedTilePosition;

    int mask = 0;

    Tilemap tilemap;

    [MenuItem("Game/RoadTile Editor")]
    static void Init()
    {
        EditorWindow.GetWindow(typeof(RoadTileEditor)).Show();
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

        if (currentScene.name != "MapScene")
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

        SceneView.onSceneGUIDelegate += OnSceneGUI;

        var map = GameObject.Find("OverlayMap");
        if (map != null)
        {
            tilemap = map.transform.Find("Road").GetComponent<Tilemap>();
        }
        else
        {
            initSuccessful = false;
            Debug.Log("Cannot find tilemap");
        }

        Tools.current = Tool.None;
        Tools.hidden = true;
    }

    private void CleanUpOnSceneChange()
    {
        SceneView.onSceneGUIDelegate -= OnSceneGUI;

        Tools.current = Tool.Move;
        Tools.hidden = false;
    }

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

        EditorGUILayout.Vector3IntField("Position: ", lastClickedTilePosition);
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        if(selectedTile != null)
        {            
            EditorGUILayout.IntField("Mask: ", mask);

            var directions = HexVectorExtensions.hexDirections;
            var location = selectedTilePos;
            var parity = location.y & 1;

            var finalMask = 0;
            
            GUILayout.BeginHorizontal();

            for (int i = 0; i < 6; i++)
            {
                var dir = new Vector3Int(directions[parity][i, 0], directions[parity][i, 1], 0);
                var pos = new Vector3Int(location.x + directions[parity][i, 0], location.y + directions[parity][i, 1], 0);

                var current = GetMask(i);

                if(!GUILayout.Toggle((mask & current) == 0, i.ToString()))
                {
                    finalMask = finalMask | current;
                }
            }

            GUILayout.EndHorizontal();

            mask = finalMask;

            if (GUILayout.Button("Set"))
            {
                Matrix4x4 matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, 0f), new Vector3(1,1,mask+1));
                tilemap.SetTransformMatrix(selectedTilePos, matrix);

                tilemap.RefreshAllTiles();
            }
        }

    }

    private int GetMask(int i)
    {
        switch (i)
        {
            case 0: //right
                return 8;
            case 1: //down
                return 4;
            case 2: //left down
                return 2;
            case 3: //left
                return 1;
            case 4: //left up
                return 32;
            case 5: //up
                return 16;
            default:
                return 0;
        }
    }

    private int GetMaskFromTile(Tilemap tilemap, Vector3Int position)
    {
        var matrix = tilemap.GetTransformMatrix(position);
        var mask = matrix.lossyScale.z - 1;

        return (int)mask;
    }

    private int GetMaskFromTile(Tile tile)
    {
        var mask = tile.color.a - .937f;

        return (int)(mask * 1000);
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
                var tile = GetMousePositionTile<RoadTile>();

                if(tile != null)
                {
                    selectedTile = tile;
                    selectedTilePos = lastClickedTilePosition;

                    mask = GetMaskFromTile(tilemap, selectedTilePos);
                }

                Repaint();
            }
            else if (Event.current.type == EventType.MouseDrag)
            {
                //SetTerritoryTile(GetMousePosition());
                //Repaint();
            }
        }
    }

    Vector3Int GetMousePosition()
    {
        if (tilemap != null)
        {
            Vector3 mousePos = Event.current.mousePosition;
            float ppp = EditorGUIUtility.pixelsPerPoint;
            mousePos.y = SceneView.currentDrawingSceneView.camera.pixelHeight - mousePos.y * ppp;
            mousePos.x *= ppp;

            Vector3 worldPos = SceneView.currentDrawingSceneView.camera.ScreenToWorldPoint(mousePos);

            return tilemap.WorldToCell(worldPos);
        }

        return new Vector3Int(0, 0, 0);
    }

    T GetMousePositionTile<T>() where T : TileBase
    {
        if (tilemap != null)
        {
            var cellPos = GetMousePosition();

            lastClickedTilePosition = cellPos;

            var tile = tilemap.GetTile<T>(cellPos);

            if (tile != null)
            {
                return tile;
            }
        }

        return null;
    }
}
