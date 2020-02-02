using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Blanket : MonoBehaviour
{
    public GameObject hole;
    public Material HoleMaterial;
    public CameraController CameraController;
    public int Score = 0;

    private Transform _thisTransform;
    private List<GameObject> _holes = new List<GameObject>();
    private List<GameObject> _patches = new List<GameObject>();
    private int _xMod = 1;
    private int _yMod = 1;
    private int _xPos = 0;
    private int _yPos = 0;
    private bool _gameOver = true;

    private void Start()
    {
        _thisTransform = GetComponent<Transform>();
        
        GameManager.Instance.RegisterGameOver(GameOver);
        GameManager.Instance.RegisterBeforeTutorial(Restart);

        Restart();
    }

    public void StartPatching()
    {
        _holes.ForEach(x => GameObject.Destroy(x));
        _patches.ForEach(x => GameObject.Destroy(x));
        _holes.Clear();
        _patches.Clear();

        for (int i = 0; i < 22; i++)
        {
            var holePosition = new Vector3(_xPos * _xMod, _yPos * _yMod, 1);

            GameObject newHole = Instantiate(hole, holePosition , Quaternion.Euler(0, 0, 30 * UnityEngine.Random.value), _thisTransform);
            newHole.name = "My Hole " + i;
            newHole.GetComponent<Hole>().Difficulty = i;
            newHole.GetComponent<MeshRenderer>().material = HoleMaterial;
            _holes.Add(newHole);

            SpiraliseHoles();
        }
    }
    
    public void ReceivePatch(GameObject patch)
    {
        if(!_gameOver)
        {
            patch.transform.position = new Vector3(patch.transform.position.x, patch.transform.position.y,0.5f);
            patch.transform.SetParent(_thisTransform, true);
            _patches.Add(patch);

            CameraController.MoveToPoint(_holes[_patches.Count].transform.position);
        } 
        else 
        {
            GameObject.Destroy(patch);
        }
    }

    private void GameOver()
    {
        _gameOver = true;
        CalculateScore(_holes, _patches);
    } 

    private void CalculateScore(List<GameObject> holes, List<GameObject> patches)
    {
        int i = 0;
        float totalRemaining = 0;
        float totalStartingHoleArea = 0;
        foreach (var hole in holes){
            float remaining = 0;
            if(patches.Count > i){
                remaining = CalculateRemainingAndOverlapArea(hole, patches[i]);
            } else {
                remaining = CalculateMeshArea(hole.GetComponent<MeshFilter>().mesh);
            }
            totalRemaining += remaining;
            totalStartingHoleArea += CalculateMeshArea(hole.GetComponent<MeshFilter>().mesh); //Same calc as above for else hole mesh area;
            i++;
        }
        float rawScore = (totalStartingHoleArea - (totalStartingHoleArea - totalRemaining));
        int finalScore = 100 - (int)(((float)totalStartingHoleArea / (float)rawScore) * 100f);

        GameManager.Instance.UpdateScore(
            System.Math.Abs(finalScore)
            );
    }

    private float CalculateRemainingAndOverlapArea(GameObject hole, GameObject patch)
    {
        float patchArea = CalculateMeshArea(patch.GetComponent<MeshFilter>().mesh);
        float holeArea = CalculateMeshArea(patch.GetComponent<MeshFilter>().mesh);
        float areaResult = holeArea - patchArea;

        //TODO: This is stuffed, please need to figure out how to factor in distance
        float distance = Vector3.Distance(
            hole.transform.position,
            patch.transform.position
        );

        if(patchArea > holeArea && patchArea < (holeArea * 1.1)){
            return 0; 
        } else {
            return System.Math.Abs(areaResult);
        }
    } 

    private float CalculateMeshArea(Mesh mesh)
    {
        var triangles = mesh.triangles;
        var vertices = mesh.vertices;

        float sum = 0.0f;

        for(int i = 0; i < triangles.Length; i += 3) {
            Vector3 corner = vertices[triangles[i]];
            Vector3 a = vertices[triangles[i + 1]] - corner;
            Vector3 b = vertices[triangles[i + 2]] - corner;

            sum += Vector3.Cross(a, b).magnitude;
        }

        return sum/2.0f;
    }

    private void Restart()
    {
        _gameOver = false;
        _xMod = 1;
        _yMod = 1;
        _xPos = 0;
        _yPos = 0;
        StartPatching();
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
