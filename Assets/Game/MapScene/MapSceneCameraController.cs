﻿using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MapSceneCameraController : MonoBehaviour
{
    private Vector3 lastDragPosition;

    public float dragSensitivity = .001f;
    public float zoomSensitivity = 5f;

    public float minSize = 1f;
    public float maxSize = 10f;

    bool startDrag = false;

    private Vector3 displacement;

    private void Start()
    {
        displacement = Camera.main.transform.position - transform.position;
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

            transform.position = new Vector3(Camera.main.transform.position.x + displacement.x,
                                             Camera.main.transform.position.y + displacement.y, 10);

            lastDragPosition = Input.mousePosition;
        }
    }

    private void Zoom()
    {
        var scrollSpeed = Input.GetAxis("Mouse ScrollWheel");

        if (scrollSpeed != 0 
            && !EventSystem.current.IsPointerOverGameObject()
            && MouseInsideScreen())
        {
            float size = Camera.main.orthographicSize;
            size -= scrollSpeed * zoomSensitivity;
            size = Mathf.Clamp(size, minSize, maxSize);
            Camera.main.orthographicSize = size;
        }
    }
}