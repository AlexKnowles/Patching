using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Blanket : MonoBehaviour
{
    public GameObject hole;
    public Material HoleMaterial;

    private Transform _thisTransform;
    private List<GameObject> _holes = new List<GameObject>();
    private List<GameObject> _patches = new List<GameObject>();
    private Vector3 _cameraTarget;
    private int _cameraXOffset = 5;
    private int _xMod = 1;
    private int _yMod = 1;
    private int _xPos = 0;
    private int _yPos = 0;
    private bool _canPatch = true;

    void Awake()
    {
        
    }

    private void ResetCamera()
    {
        _thisTransform = GetComponent<Transform>();
        _cameraTarget = new Vector3(_cameraXOffset,0,0);
        Camera.main.transform.position = _cameraTarget;
        Camera.main.orthographicSize = 5;
    }

    // Start is called before the first frame update
    public void StartPatching()
    {
        ResetCamera();
        _canPatch = true;
        _holes.ForEach(x => GameObject.Destroy(x));
        _patches.ForEach(x => GameObject.Destroy(x));
        _holes.Clear();
        _patches.Clear();

        for (int i = 0; i < 22; i++){
            hole.GetComponent<Hole>().Difficulty = i;
            var holePosition = new Vector3(_xPos * _xMod, _yPos * _yMod, 1);

            SpiraliseHoles();

            GameObject newHole = Instantiate(hole, holePosition , Quaternion.Euler(0, 0, 30 * UnityEngine.Random.value));
            newHole.name = "My Hole " + i;
            newHole.GetComponent<MeshRenderer>().material = HoleMaterial;
            newHole.transform.parent = _thisTransform;
            _holes.Add(newHole);
        }
    }
    
    public void StopPatching(){
        _canPatch = false;
        MoveCameraToOverview();
    }


    public void ReceivePatch(GameObject patch)
    {
        if(_canPatch){
            patch.transform.position = new Vector3(patch.transform.position.x, patch.transform.position.y,0.5f);
            patch.transform.SetParent(_thisTransform, true);
            _patches.Add(patch);
            _cameraTarget = _holes[_patches.Count].transform.position;
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
        if (_canPatch && Time.time > _nextCameraMove) {
            Vector3 newVector = new Vector3(
                Mathf.Lerp(Camera.main.transform.position.x, _cameraTarget.x, 6.0f * Time.deltaTime),
                Mathf.Lerp(Camera.main.transform.position.y, _cameraTarget.y, 6.0f * Time.deltaTime),
                -10
            );
            Camera.main.transform.position = newVector;
        }
    }
    
    private void MoveCameraToOverview(){
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

    private void SpiraliseHoles()
    {
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
    }
}
