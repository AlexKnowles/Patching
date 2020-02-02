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
    public Stencil Stencil;

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

            GameObject newHole = Instantiate(hole, holePosition , Quaternion.Euler(0, 0, 0), _thisTransform);
            newHole.name = "My Hole " + i;
            newHole.GetComponent<Hole>().Difficulty = i;
            newHole.GetComponent<MeshRenderer>().material = HoleMaterial;
            _holes.Add(newHole);

            SpiraliseHoles();
        }
        
        Stencil.GiveHole(_holes[0]);    
    }
    
    public void ReceivePatch(GameObject patch)
    {
        if(!_gameOver)
        {
            patch.transform.position = new Vector3(patch.transform.position.x, patch.transform.position.y,0.5f);
            patch.transform.SetParent(_thisTransform, true);
            _patches.Add(patch);

            CameraController.MoveToPoint(_holes[_patches.Count].transform.position);
            Stencil.GiveHole(_holes[_patches.Count]);   
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
        // GameManager.Instance.UpdateScore((int)(Random.value * Random.value * 100f));
        // Where the rules are made up and the points don't matter
        int i = 0;
        float totalRemaining = 0;
        float totalStartingHoleArea = 0;

        foreach (var hole in holes){
            if(patches.Count > i){
                totalRemaining += CalculateRemainingAndOverlapArea(hole, patches[i]);
            } else {
                totalRemaining += 1;
            }
            totalStartingHoleArea += 1; //Same calc as above for else hole mesh area;
            i++;
        }
        
        int finalScore = (int)(((totalStartingHoleArea - totalRemaining)/totalStartingHoleArea)*100f);
        
        if(finalScore < 0) 
            finalScore = 0;

        Debug.Log("totalStartingHoleArea:"+totalStartingHoleArea);
        Debug.Log("totalRemaining:"+totalRemaining);
        Debug.Log("final:"+finalScore);
        GameManager.Instance.UpdateScore(finalScore);
    }

    private float CalculateRemainingAndOverlapArea(GameObject hole, GameObject patch)
    {
        float patchArea = CalculateMeshArea(patch.GetComponent<MeshFilter>().mesh) * 2;
        float holeArea = CalculateMeshArea(hole.GetComponent<MeshFilter>().mesh) * 10;

        float minScoreArea = (holeArea * 0.95f);
        float maxScoreArea = (holeArea * 1.2f);

        Debug.Log("patchArea:"+patchArea);
        Debug.Log("holeArea:"+holeArea);

        if((patchArea < minScoreArea) || (patchArea > maxScoreArea))
            return 1;
        
        return 0;
    } 

    private float CalculateMeshArea(Mesh mesh)
    {
        Vector3 direction = Vector3.forward;
        var triangles = mesh.triangles;
        var vertices = mesh.vertices;

        double sum = 0.0;

        for(int i = 0; i < triangles.Length; i += 3) {
            Vector3 corner = vertices[triangles[i]];
            Vector3 a = vertices[triangles[i + 1]] - corner;
            Vector3 b = vertices[triangles[i + 2]] - corner;

            float projection = Vector3.Dot(Vector3.Cross(b, a), direction);
            if (projection > 0f)
                sum += projection;
        }

        return (float)(sum/2.0);
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
