using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class MapResourcePopup : Popup
{
    public Image icon;
    public Text amount;

    public Image fill;

    MapResource targetResource;

    private void Update()
    {
        if(targetResource != null)
        {
            SetFill((float)targetResource.amount / targetResource.maxCapacity);
        }
        else
        {
            SetFill(0);
        }
    }

    public void SetAmount(int amount)
    {
        this.amount.text = amount.ToString();
    }

    public void ResetFill()
    {
        //lastCollectionTime = Time.time;
        //fill.fillAmount = 0f;
    }

    public void SetFill(float percent)
    {
        fill.fillAmount = percent;
    }

    public void SetResource(MapResource resource, float collectionSpeed)
    {
        targetResource = resource;

        icon.sprite = resource.resourceObject.image;
        amount.text = resource.amount.ToString();

        //ResetFill();
        Show();
    }
}