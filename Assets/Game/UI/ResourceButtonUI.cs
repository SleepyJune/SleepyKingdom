﻿using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class ResourceButtonUI : MonoBehaviour
{
    public Image icon;
    
    public Text resourceName;
    public Text amount;

    public Button amountButton;

    private Country country;
    private CountryResourceType resourceType;

    public void SetResource(Country country, CountryResourceType type)
    {
        this.country = country;
        this.resourceType = type;

        string name = type.ToString();

        SpriteObject spriteObject;
        if(GameManager.instance.spriteObjectManager.spriteObjects.TryGetValue(name, out spriteObject))
        {
            icon.sprite = spriteObject.image;
        }

        amount.text = country.GetResource(type).ToString();

        country.OnResourceChange += OnResourceChange;
    }

    private void OnResourceChange(CountryResourceType type, float oldValue, float newValue)
    {        
        if(type == resourceType)
        {
            amount.text = Mathf.Round(newValue).ToString();
        }
    }
}