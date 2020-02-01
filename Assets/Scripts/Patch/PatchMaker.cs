using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatchMaker : MonoBehaviour
{
    public GameObject PatchObjectPrefab;
    public PatchCutter Cutter;
    public PatchMover Mover;
    public MockBlanket Blanket;

    public GameObject CurrentPatch {get; private set;}

    private Transform _thisTransform;

    private void Start()
    {
        _thisTransform = GetComponent<Transform>();
        CreatePatch();
    }

    public void CreatePatch()
    {
        CurrentPatch = Instantiate(PatchObjectPrefab, _thisTransform);
    }

    public void SendPatchToBlanket()
    {
        Blanket.ReceivePatch(CurrentPatch);

        CreatePatch();

        Cutter.Reset();
        Mover.Reset();
    }
}
