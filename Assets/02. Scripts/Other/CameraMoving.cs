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
        FollowToPlayer();           //플레이어의 transform을 Lerp형식으로 보간하며 따라가는 메서드
        SetRotation();              //카메라의 회전값을 계산해서 적용시키는 메서드
    }


    //플레이어의 transform을 Lerp형식으로 보간하며 따라가는 메서드
    private void FollowToPlayer()
    {
        transform.position = Vector3.Lerp
        (
            transform.position,
            _playerTransform.position + Vector3.up * 0.5f,
            Time.deltaTime * _cameraLerpSpeed
        );
    }


    //카메라의 회전값을 계산해서 적용시키는 메서드
    private void SetRotation()
    {
        _curCameraXRot += _playerInput.MousePositionDir.y * _playerStatus.Sensitivity;
        _curCameraXRot = Mathf.Clamp(_curCameraXRot, _playerStatus.MinCurXRot, _playerStatus.MaxCurXRot);
        transform.rotation = Quaternion.Euler
            (Vector3.up * _playerTransform.eulerAngles.y - _curCameraXRot * Vector3.right);
    }
}
