using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class DeployUnitManager : MonoBehaviour
{
    BattleUnitManager battleUnitManager;

    public DeployUnitButton buttonPrefab;
    public Transform buttonList;

    private void Start()
    {
        battleUnitManager = GetComponent<BattleUnitManager>();

        Initialize();
    }

    void Initialize()
    {
        foreach(var unitObj in GameManager.instance.gamedatabaseManager.battleUnitObjects.Values)
        {
            var newButton = Instantiate(buttonPrefab, buttonList);
            newButton.Initialize(unitObj, this);
        }
    }

    public void OnUnitButtonPressed(BattleUnitObject unit)
    {
        Country country = GameManager.instance.globalCountryManager.myCountry;
        if (country.population >= unit.populationCost)
        {
            var team = UnityEngine.Random.Range(0, 100) % 2 == 0 ? BattleUnitTeam.Player : BattleUnitTeam.Computer;
            battleUnitManager.CreateUnit(unit, team);

            country.population -= unit.populationCost;
        }
    }
}