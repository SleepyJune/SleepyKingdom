using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class CastleSelectPopup : Popup
{
    public Transform buildingList;

    public CastleSelectItem itemPrefab;

    public CreateCountryPopup createCountryPrefab;

    [NonSerialized]
    public Vector3Int selectedPosition;

    [NonSerialized]
    public CastleObject selectedCastle;

    private void Start()
    {
        foreach (var castle in GameManager.instance.gamedatabaseManager.castleObjects.Values)
        {
            var newItem = Instantiate(itemPrefab, buildingList);

            newItem.SetBuilding(castle, this);

            //buildings.Add(newItem);
        }
    }

    public void ShowCreateCountryPopup(CastleObject castle)
    {
        var canvas = GameObject.Find("Canvas");

        if (canvas != null)
        {
            selectedCastle = castle;

            var newPopup = Instantiate(createCountryPrefab, canvas.transform);
            
            newPopup.OnClose += OnNamePopupClose;
        }
    }

    private void OnNamePopupClose(Popup popup)
    {
        popup.OnClose -= OnNamePopupClose;

        var createPopup = popup as CreateCountryPopup;
        if(createPopup != null)
        {
            var name = createPopup.inputText;

            GameManager.instance.globalCountryManager.OnCreateCountry(name, selectedCastle, selectedPosition);
        }

        Close();
    }

    public void ShowPopup(TowerSlotController slot)
    {
        //this.slot = slot;

        Show();
    }

}