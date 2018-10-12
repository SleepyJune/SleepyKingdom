using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class CountryListManager : MonoBehaviour
{
    public Transform countryListParent;

    public GameObject countryButtonPrefab;

    private GameState gameState;

    private void Start()
    {
        gameState = GameManager.instance.gameStateManager.gameState;
    }

    public void GenerateCountry()
    {
        var newCountry = Country.Generate();

        gameState.AddCountry(newCountry);

        UpdateList();
    }

    public void ViewCountry(Country country)
    {        
        GameManager.instance.sceneChanger.ChangeScene(country);
    }

    public void UpdateList()
    {
        foreach(Transform transform in countryListParent)
        {
            Destroy(transform.gameObject);
        }

        foreach(var country in gameState.GetCountries())
        {
            var newButton = Instantiate(countryButtonPrefab, countryListParent);

            var buttonTrans = newButton.transform.Find("Button");

            buttonTrans.Find("Text").GetComponent<Text>().text = country.countryName;

            buttonTrans.GetComponent<Button>().onClick.AddListener(() => ViewCountry(country));
        }
    }
}