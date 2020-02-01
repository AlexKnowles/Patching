using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MockPatchGenerator : MonoBehaviour
{
    public Blanket Blanket;
    public GameObject Patch;
    public Material PatchMaterial;
    private int _patchCount = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    private float nextActionTime = 1.0f;
    public float period = 1.5f;
    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextActionTime ) {
            nextActionTime += period;
            _patchCount++;
            GeneratePatch();
        }
    }
    void GeneratePatch()
    {
        Debug.Log("Generating patch");
        Patch.GetComponent<Hole>().Difficulty = 1;
        GameObject newPatch = Instantiate(Patch, Camera.main.transform.position, Quaternion.Euler(0, 0, 0));
        newPatch.name = "My Patch "+ _patchCount;
        newPatch.GetComponent<MeshRenderer>().material = PatchMaterial;
        newPatch.transform.position = new Vector3(newPatch.transform.position.x - 5, newPatch.transform.position.y, -1);
        Blanket.ReceivePatch(newPatch);
    }
}
