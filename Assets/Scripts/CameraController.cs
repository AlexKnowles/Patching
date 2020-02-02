using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{    
    private Transform _thisTransform;
    private Vector3 _cameraTarget;
    private int _cameraXOffset = 5;
    private float _nextCameraMove = 0.0f;
    private float _timeToKill = 0.2f;
    private bool _gameOver;

    private void Start()
    {
        _thisTransform = GetComponent<Transform>();

        GameManager.Instance.RegisterGameOver(GameOver);
        GameManager.Instance.RegisterBeforeTutorial(Restart);

        Restart();
    }

    private void LateUpdate()
    {
        if (!_gameOver && Time.time > _nextCameraMove) 
        {
            Vector3 newVector = new Vector3(
                Mathf.Lerp(_thisTransform.position.x, _cameraTarget.x, 6.0f * Time.deltaTime),
                Mathf.Lerp(_thisTransform.position.y, _cameraTarget.y, 6.0f * Time.deltaTime),
                -10
            );
            _thisTransform.position = newVector;
        }
        else if(_gameOver)
        {
            MoveCameraToOverview();
        }
    }

    public void ResetCamera()
    {
        _cameraTarget = new Vector3(_cameraXOffset, 0, 0);
        _thisTransform.position = _cameraTarget;
        Camera.main.orthographicSize = 5;
    }    

    public void MoveCameraToOverview()
    {
        Vector3 newVector = new Vector3(
            Mathf.Lerp(_thisTransform.position.x, 0, 3.0f * Time.deltaTime),
            Mathf.Lerp(_thisTransform.position.y, 0, 3.0f * Time.deltaTime),
            -10
        );

        if(Camera.main.orthographicSize < 28.0f)
        {
            Camera.main.orthographicSize += 0.1f;
        }

        _thisTransform.position = newVector;
    }

    public void MoveToPoint(Vector3 newTarget)
    {    
        _cameraTarget = newTarget;
        _cameraTarget.x = _cameraTarget.x + _cameraXOffset;
        _nextCameraMove = Time.time + _timeToKill;
    }

    private void GameOver()
    {
        _gameOver = true;
    }
    private void Restart()
    {
        _gameOver = false;
        ResetCamera();
    }
}
