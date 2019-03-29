using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MapSceneCameraController : MonoBehaviour
{
    public float dragSensitivity = .001f;
    public float zoomSensitivity = 5f;

    public float minSize = 1f;
    public float maxSize = 10f;

    bool isDragging = false;

    [NonSerialized]
    public bool setCenterOnShipWhenMoving = false;

    private Vector3 displacement;

    MapUnitManager unitManager;

    private void Start()
    {
        displacement = Camera.main.transform.position - transform.position;

        unitManager = GetComponent<MapUnitManager>();
    }

    public bool MouseInsideScreen()
    {
        #if UNITY_EDITOR
        if (Input.mousePosition.x <= 0 || Input.mousePosition.y <= 0 || Input.mousePosition.x > Handles.GetMainGameViewSize().x || Input.mousePosition.y > Handles.GetMainGameViewSize().y)
        {
            return false;
        }
        #else
        if (Input.mousePosition.x <= 0 || Input.mousePosition.y <= 0 || Input.mousePosition.x > Screen.width || Input.mousePosition.y > Screen.height)
        {
            return false;
        }
        #endif
        else
        {
            return true;
        }
    }

    private void LateUpdate()
    {
        Zoom();

        if (unitManager.myShip.isMoving && setCenterOnShipWhenMoving)
        {
            CenterMyShip();
        }
    }

    public void OnMouseDragEvent(TouchInput input)
    {
        setCenterOnShipWhenMoving = false;

        isDragging = true;

        Vector3 delta = Camera.main.ScreenToWorldPoint(input.previousPosition) - Camera.main.ScreenToWorldPoint(input.position);

        //var sensitivity = dragSensitivity * Camera.main.orthographicSize;

        Camera.main.transform.Translate(delta.x, delta.y, 0, Space.World);
    }

    public void OnMouseDragEndEvent(TouchInput input)
    {
        isDragging = false;
    }

    public void OnMouseDragEvent2(TouchInput input)
    {
        Vector3 delta = input.previousPosition - input.position;

        var sensitivity = dragSensitivity * Camera.main.orthographicSize;

        Camera.main.transform.Translate(delta.x * sensitivity, delta.y * sensitivity, 0, Space.World);
    }

    private void Zoom()
    {
        var scrollSpeed = Input.GetAxis("Mouse ScrollWheel");

        if (scrollSpeed != 0
            && MouseInsideScreen())
        {
            float size = Camera.main.orthographicSize;
            size -= scrollSpeed * zoomSensitivity;
            size = Mathf.Clamp(size, minSize, maxSize);
            Camera.main.orthographicSize = size;
        }
    }

    public void CenterMyShip()
    {
        CenterOn(MapSceneManager.instance.unitManager.myShip);
    }

    public void CenterOn(MapUnit unit)
    {
        var pos = unit.transform.position;

        Camera.main.transform.position = new Vector3(pos.x, pos.y, Camera.main.transform.position.z);
    }
}