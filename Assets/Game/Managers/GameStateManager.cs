using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    [NonSerialized]
    public GameState gameState;

    private void Awake()
    {
        gameState = GameState.Load();

        if(gameState.myCountry == null || gameState.myCountry.name == "")
        {
            var currentScene = SceneManager.GetActiveScene();

            if(currentScene.name != "CreationScene")
            {
                GameManager.instance.sceneChanger.ChangeScene(SceneType.NewGame);
                Destroy(gameObject);
            }
        }
    }

    public void NewState(string countryName)
    {
        gameState = new GameState(countryName);
        Save();
    }

    public void Save()
    {
        gameState.Save();
    }
}