using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatchMaker : MonoBehaviour
{
    public GameObject PatchObjectPrefab;
    public PatchCutter Cutter;
    public PatchMover Mover;
    public Blanket Blanket;

    public GameObject CurrentPatch {get; private set;}

    private Transform _thisTransform;

    private void Awake()
    {
        _thisTransform = GetComponent<Transform>();
        BeforeTutorial();
    }

    private void Start()
    {
        GameManager.Instance.RegisterBeforeTutorial(BeforeTutorial);
    }

    public void CreatePatch()
    {
        CurrentPatch = Instantiate(PatchObjectPrefab, _thisTransform);

        Cutter.NewPatch();
        Mover.Restart();
    }

    public void SendPatchToBlanket()
    {
        Blanket.ReceivePatch(CurrentPatch);
        CreatePatch();
    }

    private void BeforeTutorial()
    {
        if( CurrentPatch != null)
            GameObject.Destroy(CurrentPatch);

        CreatePatch();
    }
}
