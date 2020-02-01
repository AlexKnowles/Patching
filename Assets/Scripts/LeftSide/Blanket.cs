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
    private int _patchNumber = 0;
    public Material HoleMaterial;

    private Vector3 _cameraTarget;

    private int _cameraXOffset = 5;

    private int _xMod = 1;
    private int _yMod = 1;

    private int _xPos = 0;
    private int _yPos = 0;

    public float EndTime;

    void Awake()
    {
        _thisTransform = GetComponent<Transform>();
        _cameraTarget = new Vector3(_cameraXOffset,0,0);
        Camera.main.transform.position = _cameraTarget;
        EndTime = Time.time + 20.0f;
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        _holes = new List<GameObject>();
        _patches = new List<GameObject>();
        _patchNumber = 0;

        for (int i = 0; i < 22; i++){
            hole.GetComponent<Hole>().Difficulty = i;
            var holePosition = new Vector3(
                _xPos * _xMod, 
                _yPos * _yMod, 
                0);

            if(_xPos < 7){
                if(_xMod == 1 && _yMod == 1){
                    _xMod = -1;
                    _xPos = _xPos + 6;
                    _yPos = _yPos + 6;
                } else if(_xMod == -1 && _yMod == 1){
                    _yMod = -1;
                } else if(_xMod == -1 && _yMod == -1){
                    _xMod = 1;
                } else if(_xMod == 1 && _yMod == -1){
                    _yMod = 1;
                }
            } else {
                if(_xMod == 1 && _yMod == 1){
                    _xMod = 0;
                    _xPos = _xPos + 6;
                    _yPos = _yPos + 6;
                } else if(_xMod == 0 && _yMod == 1){
                    _xMod = -1;
                } else if(_xMod == -1 && _yMod == 1){
                    _yMod = 0;
                } else if(_xMod == -1 && _yMod == 0){
                    _yMod = -1;
                } else if(_xMod == -1 && _yMod == -1){
                    _xMod = 0;
                } else if(_xMod == 0 && _yMod == -1){
                    _xMod = 1;
                } else if(_xMod == 1 && _yMod == -1){
                    _yMod = 0;
                } else if(_xMod == 1 && _yMod == 0){
                    _yMod = 1;
                }
            }



            GameObject newHole = Instantiate(hole, holePosition , Quaternion.Euler(0, 0, 30 * UnityEngine.Random.value));
            newHole.name = "My Hole " + i;
            newHole.GetComponent<MeshRenderer>().material = HoleMaterial;
            newHole.transform.parent = _thisTransform;
            _holes.Add(newHole);
        }
    }

    public void ReceivePatch(GameObject patch)
    {
        if(Time.time < EndTime){
            Debug.Log("Patches Count: " + _patches.Count);
            Debug.Log("Holes Count: " + _holes.Count);
            patch.transform.SetParent(_thisTransform, true);
            _patches.Add(patch);
            _patchNumber++;
            _cameraTarget = _holes[_patchNumber].transform.position;
            _cameraTarget.x = _cameraTarget.x + _cameraXOffset;
            _nextCameraMove = Time.time + _timeToKill;
        } else {
            GameObject.Destroy(patch);
        }
    }

    private float _nextCameraMove = 0.0f;
    private float _timeToKill = 0.2f;

    void LateUpdate()
    {
        if (Time.time < EndTime && Time.time > _nextCameraMove ) {
            Vector3 newVector = new Vector3(
                Mathf.Lerp(Camera.main.transform.position.x, _cameraTarget.x, 6.0f * Time.deltaTime),
                Mathf.Lerp(Camera.main.transform.position.y, _cameraTarget.y, 6.0f * Time.deltaTime),
                -10
            );
            Camera.main.transform.position = newVector;
        }

        if(Time.time > EndTime){
            Vector3 newVector = new Vector3(
                Mathf.Lerp(Camera.main.transform.position.x, 0, 3.0f * Time.deltaTime),
                Mathf.Lerp(Camera.main.transform.position.y, 0, 3.0f * Time.deltaTime),
                -10
            );
            if(Camera.main.orthographicSize < 35.0f)
            {
                Camera.main.orthographicSize += 0.1f;
            }
            Camera.main.transform.position = newVector;
        }
    }
}
