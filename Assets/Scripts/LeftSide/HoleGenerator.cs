using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleGenerator : MonoBehaviour
{
    public GameObject hole;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 50; i++){
            hole.GetComponent<Hole>().Difficulty = i + 3;
            Instantiate(hole, new Vector3(2 * i, 2 * i, 0), Quaternion.Euler(0, 0, 30 * i));
        }
    }

    // Update is called once per frame
    //void Update()
    //{
    //    
    //}
}
