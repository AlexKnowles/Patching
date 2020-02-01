using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Blanket Blanket;
    public float RemainingTime {
        get { return EndTime - Time.time > 0 ? EndTime - Time.time : 0; }
    }
    public float EndTime;
    public float StartTime;
    private float _gameDuration = 20.0f;

    // Start is called before the first frame update
    void Start()
    {
        StartGame();
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time > EndTime)
        {
            Blanket.StopPatching();
        }
    }

    public void StartGame() 
    {
        StartTime = Time.time;
        EndTime = StartTime + _gameDuration;
        Blanket.StartPatching();
    }
}
