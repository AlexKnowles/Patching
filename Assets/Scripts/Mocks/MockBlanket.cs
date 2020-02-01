using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MockBlanket : MonoBehaviour
{
    public void ReceivePatch(GameObject patchObject)
    {
        patchObject.GetComponent<Transform>().parent = GetComponent<Transform>();
    }
}
