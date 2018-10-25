using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum UnitCommandType
{
    Attack = 0,
    Inspect = 1,
    Move = 2,
    Trade = 3,

    None = -1,
}

public class ActionCircleController : Popup
{
    [NonSerialized]
    public CastleUnit castle;

    public UnitManager unitManager;

    private void Start()
    {
        
    }

    public void SetCastle(CastleUnit target)
    {
        castle = target;

        transform.position = Input.mousePosition;

        Show();
    }

    public void OnPointerEnter(int actionIndex)
    {
        var actionType = (UnitCommandType)actionIndex;

        if(actionType == UnitCommandType.Attack)
        {
            Debug.Log("Attack");
        }
        else if (actionType == UnitCommandType.Move)
        {

        }
    }

    public void OnButtonClicked(int actionIndex)
    {
        var actionType = (UnitCommandType)actionIndex;

        unitManager.OnActionCircleButtonClick(castle, actionType);

        Hide();
    }
}