using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
    public GameObject GameManager;
    public GameObject Timer;
    public GameObject ResetButton;
    public float TimeRemaining {
        get { return GameManager.GetComponent<GameManager>().RemainingTime; }
    }
    public bool Playing {
        get { return TimeRemaining > 0; }
    }
    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void ResetGame()
    {
        GameManager.GetComponent<GameManager>().StartGame();
    }

    public void Update(){
        if(TimeRemaining == 0){
            ResetButton.SetActive(true);
        } else {
            ResetButton.SetActive(false);
        }
        Timer.GetComponent<Text>().text = string.Format("{0:0.00}", TimeRemaining);
    }
}
