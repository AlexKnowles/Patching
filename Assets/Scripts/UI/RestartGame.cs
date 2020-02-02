using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RestartGame: MonoBehaviour
{
    private Image _image;
    private Button _button;
 
    private void Start()
    {
        _image = GetComponent<Image>();
        _button = GetComponent<Button>();

        GameManager.Instance.RegisterGameOver(GameOver);
        GameManager.Instance.RegisterBeforeTutorial(Restart);

        Restart();
    }

    private void Restart()
    {
        _image.enabled = false;
        _button.enabled = false;
    }

    private void GameOver()
    {
        _image.enabled = true;
        _button.enabled = true;
    }
}
