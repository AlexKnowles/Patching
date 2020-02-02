using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject Tutorial;
    public int Score { get; private set; }

    private List<Action> _gameOverMethods;
    private List<Action> _restartMethods;
    private List<Action> _beforeTutorialMethods;
    private bool _showingTutorial = true;

    private void Awake()
    {
        Instance = this;
        _gameOverMethods = new List<Action>();
        _restartMethods = new List<Action>();
        _beforeTutorialMethods = new List<Action>();
    }
    

    private void Update() 
    {
        if(_showingTutorial && Input.GetMouseButton(0))
            Restart();
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
        HideTutorial();

        foreach(Action restartMethod in _restartMethods)
        {
            restartMethod();
        }
    }

    public void LoadTutorial()
    {
        Score = 0;
        ShowTutorial();

        foreach(Action beforeTutorialMethod in _beforeTutorialMethods)
        {
            beforeTutorialMethod();
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
    public void RegisterBeforeTutorial(Action method)
    {
        _beforeTutorialMethods.Add(method);
    }

    private void ShowTutorial()
    {
        _showingTutorial = true;
        Tutorial.SetActive(true);
    }
    private void HideTutorial()
    {
        _showingTutorial = false;
        Tutorial.SetActive(false);
    }
    public void UpdateScore(int score){
        Score = score;
    }
}
