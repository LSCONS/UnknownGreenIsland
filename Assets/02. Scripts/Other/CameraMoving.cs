using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMoving : MonoBehaviour
{
    private Transform playerTransform;
    private PlayerInput playerInput;
    private PlayerStatus playerStatus;
    private float curCameraXRot;
    private float cameraLerpSpeed = 30f;

    private void OnValidate()
    {
        playerTransform = "Player".GetComponentNameDFS<Transform>();
        playerInput = "Player".GetComponentNameDFS<PlayerInput>();
        playerStatus = "Player".GetComponentNameDFS<PlayerStatus>();
    }


    private void LateUpdate()
    {
        FollowToPlayer();           //플레이어의 transform을 Lerp형식으로 보간하며 따라가는 메서드
        if (!(playerInput.IsInventory))
        {
            SetRotation();          //카메라의 회전값을 계산해서 적용시키는 메서드
        }

    }


    //플레이어의 transform을 Lerp형식으로 보간하며 따라가는 메서드
    private void FollowToPlayer()
    {
        transform.position = Vector3.Lerp
        (
            transform.position,
            playerTransform.position + Vector3.up * 0.5f,
            Time.deltaTime * cameraLerpSpeed
        );
    }


    //카메라의 회전값을 계산해서 적용시키는 메서드
    private void SetRotation()
    {
        curCameraXRot += playerInput.MousePositionDir.y * playerStatus.Sensitivity;
        curCameraXRot = Mathf.Clamp(curCameraXRot, playerStatus.MinCurXRot, playerStatus.MaxCurXRot);
        transform.rotation = Quaternion.Euler
            (Vector3.up * playerTransform.eulerAngles.y - curCameraXRot * Vector3.right);
    }
}
