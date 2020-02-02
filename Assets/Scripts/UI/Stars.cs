using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stars : MonoBehaviour
{
 
    private void Start()
    {
        GameManager.Instance.RegisterGameOver(GameOver);
        GameManager.Instance.RegisterBeforeTutorial(Restart);

        Restart();
    }

    private void Restart()
    {
        gameObject.SetActive(false);
    }

    private void GameOver()
    {
        gameObject.SetActive(true);
    }
}
