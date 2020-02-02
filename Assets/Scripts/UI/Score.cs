using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public int StarStart = 0;
    public int StarEnd = 0;
    private void Update()
    {
        
        if(GameManager.Instance.Score != 0){
            if(GameManager.Instance.Score > StarEnd){
                GetComponent<Image>().fillAmount = 1f;
            } else {
                int remaining = GameManager.Instance.Score - StarStart;
                float fraction = (float)remaining / (float)(StarEnd - StarStart);
                GetComponent<Image>().fillAmount = fraction;

            }
        } else {
            GetComponent<Image>().fillAmount = 0f;
        }
    }
}
