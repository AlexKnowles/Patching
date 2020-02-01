using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Blanket : MonoBehaviour
{
    public GameObject hole;

    private Transform _thisTransform;

    private List<GameObject> _holes;
    private List<GameObject> _patches;
    public Material HoleMaterial;

    private Vector3 _cameraTarget;

    private int _cameraXOffset = 5;

    void Awake()
    {
        _thisTransform = GetComponent<Transform>();
        _cameraTarget = new Vector3(_cameraXOffset,0,0);
        Camera.main.transform.position = _cameraTarget;
    }

    // Start is called before the first frame update
    void Start()
    {
        _holes = new List<GameObject>();
        _patches = new List<GameObject>();
        for (int i = 0; i < 20; i++){
            hole.GetComponent<Hole>().Difficulty = i;
            GameObject newHole = Instantiate(hole, new Vector3(6 * i, 6 * i, 0), Quaternion.Euler(0, 0, 30 * i));
            newHole.name = "My Hole " + i;
            newHole.GetComponent<MeshRenderer>().material = HoleMaterial;
            newHole.transform.parent = _thisTransform;
            _holes.Add(newHole);
        }
    }

    public void ReceivePatch(GameObject patch)
    {
        patch.transform.SetParent(_thisTransform, true);
        _patches.Add(patch);
        //TODO: Move camera to new hole
        _cameraTarget = _holes[_patches.Count].transform.position;
        _cameraTarget.x = _cameraTarget.x + _cameraXOffset;
        nextCameraMove = Time.time + timeToKill;
    }

    private float nextCameraMove = 0.0f;
    private float timeToKill = 0.25f;

    void LateUpdate()
    {
        if (Time.time > nextCameraMove ) {
            Vector3 newVector = new Vector3(
                Mathf.Lerp(Camera.main.transform.position.x, _cameraTarget.x, 5.0f * Time.deltaTime),
                Mathf.Lerp(Camera.main.transform.position.y, _cameraTarget.y, 5.0f * Time.deltaTime),
                -10
            );
            Camera.main.transform.position = newVector;
        }
    }
}
