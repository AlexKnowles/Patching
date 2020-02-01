using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blanket : MonoBehaviour
{
    public GameObject hole;

    private Transform _thisTransform;

    // Start is called before the first frame update
    void Start()
    {
        _thisTransform = GetComponent<Transform>();
        for (int i = 0; i < 20; i++){
            hole.GetComponent<Hole>().Difficulty = i;
            GameObject newHole = Instantiate(hole, new Vector3(6 * i, 6 * i, 0), Quaternion.Euler(0, 0, 30 * i));
            newHole.name = "My Hole " + i;
            newHole.transform.parent = _thisTransform;
        }
    }

    public void ReceivePatch(GameObject patch)
    {
        patch.transform.parent = _thisTransform;
    }

    //Update is called once per frame
    void Update()
    {
        
    }
}
