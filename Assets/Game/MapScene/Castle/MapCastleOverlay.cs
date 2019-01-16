using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class MapCastleOverlay : Popup
{
    MapCastleUnit castle;

    public Text populationText;

    RectTransform topBar;

    private void Start()
    {
        topBar = panel.transform.Find("TopBar").GetComponent<RectTransform>();
    }

    private void Update()
    {
        if(castle != null)
        {
            transform.position = castle.transform.position;
        }
    }

    public void SetCastle(MapCastleUnit target)
    {
        if(target == null)
        {
            castle = null;
            Hide();

            return;
        }

        castle = target;

        var height = castle.render.sprite.bounds.size.y * 10.0f;
        
        topBar.anchoredPosition = new Vector2(topBar.anchoredPosition.x, height);

        populationText.text = NumberTextFormater.FormatNumber(castle.country.population);

        transform.position = castle.transform.position;

        Show();
    }
}
