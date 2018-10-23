using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    [NonSerialized]
    public GameState gameState;

    private void Awake()
    {
        gameState = GameState.Load();
    }

    public void Save()
    {
        gameState.Save();
    }
}