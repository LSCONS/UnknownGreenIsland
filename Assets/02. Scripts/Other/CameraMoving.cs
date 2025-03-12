using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMoving : MonoBehaviour
{
    private Transform _playerTransform;
    private PlayerInput _playerInput;
    private PlayerStatus _playerStatus;
    private float _curCameraXRot;
    private float _cameraLerpSpeed = 20f;

    private void OnValidate()
    {
        _playerTransform = "Player".GetComponentNameDFS<Transform>();
        _playerInput = "Player".GetComponentNameDFS<PlayerInput>();
        _playerStatus = "Player".GetComponentNameDFS<PlayerStatus>();
    }


    private void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, _playerTransform.position + Vector3.up * 1.5f, Time.deltaTime * _cameraLerpSpeed);
        _curCameraXRot += _playerInput.MousePositionDir.y * _playerStatus.Sensitivity;
        _curCameraXRot = Mathf.Clamp(_curCameraXRot, _playerStatus.MinCurXRot, _playerStatus.MaxCurXRot);
        Vector3 resultEuler = Vector3.up * _playerTransform.eulerAngles.y - _curCameraXRot * Vector3.right;
        transform.rotation = Quaternion.Euler(resultEuler);
    }
}
