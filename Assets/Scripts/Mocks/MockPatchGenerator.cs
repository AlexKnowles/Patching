using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MockPatchGenerator : MonoBehaviour
{
    public Blanket Blanket;
    public GameObject Patch;
    // Start is called before the first frame update
    void Start()
    {
        Patch.GetComponent<Hole>().Difficulty = 2;
        GameObject newPatch = Instantiate(Patch, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
        newPatch.name = "My Patch";
        newPatch.transform.parent = GetComponent<Transform>();
        Blanket.ReceivePatch(newPatch);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
