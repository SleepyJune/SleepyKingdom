using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class TowerFloorItem : MonoBehaviour
{
    public Text floorName;

    [NonSerialized]
    public TowerFloor towerFloor;

    public void SetFloor(TowerFloor floor)
    {
        towerFloor = floor;

        floorName.text = "Floor " + (floor.floorNum+1);

        var background = transform.Find("Background");

        var rectTransform = background.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(20 * floor.floorSize, 100);// rectTransform.sizeDelta.y);
    }

    public void OnFloorPress()
    {
        GameManager.instance.sceneChanger.ChangeScene(towerFloor);
    }
}