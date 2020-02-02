using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public float GameLength = 20f;

    private Slider _slider;
    private float _timePassed = 0;

    private void Start()
    {
        GameManager.Instance.RegisterRestart(Restart);
        GameManager.Instance.RegisterBeforeTutorial(BeforeTutorial);
     
        _slider = GetComponent<Slider>();

        BeforeTutorial();
    }

    private void Update()
    {
        if(_timePassed >= GameLength)
            return;

        _timePassed += Time.deltaTime;
        _slider.value = (1 - (_timePassed / GameLength));
        
        if(_timePassed >= GameLength)
        {
            GameManager.Instance.GameOver();
        }     
    }

    private void Restart() 
    {
        _timePassed = 0;
    }
    private void BeforeTutorial()
    {
        _timePassed = GameLength;
        _slider.value = _timePassed;
    }
}
