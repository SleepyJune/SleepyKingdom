using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;

public class MapSceneCameraController : MonoBehaviour
{
    private Vector3 lastDragPosition;

    public float dragSensitivity = .001f;
    public float zoomSensitivity = 5f;

    public float minSize = 1f;
    public float maxSize = 10f;

    bool startDrag = false;

    private void LateUpdate()
    {
        Zoom();
    }
    
    private void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            lastDragPosition = Input.mousePosition;
            startDrag = true;
        }
        else
        {
            startDrag = false;
        }
    }

    private void OnMouseUp()
    {
        startDrag = false;
    }

    private void OnMouseDrag()
    {
        if (startDrag)
        {
            Vector3 delta = lastDragPosition - Input.mousePosition;

            var sensitivity = dragSensitivity * Camera.main.orthographicSize;

            Camera.main.transform.Translate(delta.x * sensitivity, delta.y * sensitivity, 0, Space.World);

            lastDragPosition = Input.mousePosition;
        }
    }

    private void Zoom()
    {
        var scrollSpeed = Input.GetAxis("Mouse ScrollWheel");

        if (scrollSpeed != 0 && !EventSystem.current.IsPointerOverGameObject())
        {
            float size = Camera.main.orthographicSize;
            size -= scrollSpeed * zoomSensitivity;
            size = Mathf.Clamp(size, minSize, maxSize);
            Camera.main.orthographicSize = size;
        }
    }
}