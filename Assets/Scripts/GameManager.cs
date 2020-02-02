using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public float EndTime;
    public float StartTime;

    private List<Action> _gameOverMethods;
    private List<Action> _restartMethods;

    private void Awake()
    {
        Instance = this;
        _gameOverMethods = new List<Action>();
        _restartMethods = new List<Action>();
    }
    
    public void GameOver()
    {
        foreach(Action gameOverMethod in _gameOverMethods)
        {
            gameOverMethod();
        }
    }

    public void Restart()
    {
        foreach(Action restartMethod in _restartMethods)
        {
            restartMethod();
        }
    }

    public void RegisterGameOver(Action method)
    {
        _gameOverMethods.Add(method);
    }

    public void RegisterRestart(Action method)
    {
        _restartMethods.Add(method);
    }
}
