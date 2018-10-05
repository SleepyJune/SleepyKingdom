using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class ResourceButtonUI : MonoBehaviour
{
    public Sprite resourceImage;

    public Text resourceName;
    public Text amount;

    public Button amountButton;

    private Country country;
    private CountryResourceType resourceType;

    public void SetResource(Country country, CountryResourceType type)
    {
        this.country = country;
        this.resourceType = type;

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