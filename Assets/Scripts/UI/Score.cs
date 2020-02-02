using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    private void Update()
    {
        if(GameManager.Instance.Score != 0){
            GetComponent<Text>().text = GameManager.Instance.Score.ToString();
        } else {
            GetComponent<Text>().text = "";
        }
    }
}
