using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TouchInputManager : MonoBehaviour
{
    //public static TouchInputManager instance = null;

    GraphicRaycaster graphicRaycaster;

    public Dictionary<int, TouchInput> inputs;

    public delegate void Callback(TouchInput input);
    public event Callback touchStart;
    public event Callback touchMove;
    public event Callback touchEnd;

    public bool useMouse = false;

    Vector3 lastMousePosition;

    void Awake()
    {
        /*if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }*/

        if (Application.isMobilePlatform)
        {
            useMouse = false;
        }

        if (!useMouse)
        {
            Input.simulateMouseWithTouches = false;
        }

        inputs = new Dictionary<int, TouchInput>();
    }

    private void OnEnable()
    {
        graphicRaycaster = null;

        var canvas = GameObject.Find("Canvas");
        if(canvas != null)
        {
            graphicRaycaster = canvas.GetComponent<GraphicRaycaster>();
        }
    }

    void Update()
    {
        if (useMouse && Input.mousePresent)
        {
            if (Input.GetMouseButtonDown(0))
            {
                var touchData = new Touch();
                touchData.fingerId = -1;
                touchData.position = Input.mousePosition;
                touchData.phase = TouchPhase.Began;

                lastMousePosition = Input.mousePosition;

                TouchInput newInput;
                if (inputs.TryGetValue(touchData.fingerId, out newInput))
                {
                    newInput.startPosition = touchData.position;
                }
                else
                {
                    newInput = new TouchInput(touchData);
                    inputs.Add(newInput.id, newInput);
                }

                if (IsPointOverGameUI(newInput))
                {
                    newInput.isUIInteraction = true;
                }

                if (touchStart != null)
                {
                    touchStart(newInput);
                }
            }

            if (Input.GetMouseButton(0) && Input.mousePosition != lastMousePosition)
            {
                var touchData = new Touch();
                touchData.fingerId = -1;
                touchData.position = Input.mousePosition;
                touchData.phase = TouchPhase.Moved;

                var newInput = UpdateTouchPosition(touchData);

                lastMousePosition = Input.mousePosition;
                
                if (touchMove != null)
                {
                    touchMove(newInput);
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                var touchData = new Touch();
                touchData.fingerId = -1;
                touchData.position = Input.mousePosition;
                touchData.phase = TouchPhase.Ended;

                var newInput = UpdateTouchPosition(touchData);

                lastMousePosition = Input.mousePosition;

                if (touchEnd != null)
                {
                    touchEnd(newInput);
                }

                inputs.Remove(touchData.fingerId);
            }
        }

        if (Input.touchCount > 0)
        {
            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    TouchInput newInput;
                    if (inputs.TryGetValue(touch.fingerId, out newInput))
                    {
                        newInput.startPosition = touch.position;
                    }
                    else
                    {
                        newInput = new TouchInput(touch);
                        inputs.Add(newInput.id, newInput);
                    }

                    if (IsPointOverGameUI(newInput))
                    {
                        newInput.isUIInteraction = true;
                        //may not work...
                    }

                    if (touchStart != null)
                    {
                        touchStart(newInput);
                    }
                }

                if (touch.phase == TouchPhase.Moved)
                {
                    var newInput = UpdateTouchPosition(touch);
                      
                    if (touchMove != null)
                    {
                        touchMove(newInput);
                    }
                }

                if (touch.phase == TouchPhase.Canceled || touch.phase == TouchPhase.Ended)
                {
                    var newInput = UpdateTouchPosition(touch);

                    if (touchEnd != null)
                    {
                        touchEnd(newInput);
                    }

                    inputs.Remove(touch.fingerId);
                }
            }
        }
    }

    private bool IsPointOverGameUI(TouchInput input)
    {
        if (graphicRaycaster != null)
        {
            var pointEventData = new PointerEventData(EventSystem.current);
            pointEventData.position = input.position;

            List<RaycastResult> results = new List<RaycastResult>();

            graphicRaycaster.Raycast(pointEventData, results);

            if (results.Count > 0)
            {
                return true;
            }
        }

        return false;
    }

    public bool isMultiTouch()
    {
        return inputs.Count > 1;
    }

    TouchInput UpdateTouchPosition(Touch touch)
    {
        TouchInput newInput;
        if (inputs.TryGetValue(touch.fingerId, out newInput))
        {
            newInput.previousPosition = newInput.position;
            newInput.position = touch.position;
        }
        else
        {
            newInput = new TouchInput(touch);
            inputs.Add(newInput.id, newInput);
        }

        if(Vector2.Distance(newInput.position, newInput.startPosition) > 50)
        {
            newInput.isDrag = true;
        }

        return newInput;
    }
}
