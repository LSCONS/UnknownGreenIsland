using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    private PlayerInput _input;
    private CharacterController chrConPlayer;
    private PlayerStatus _playerStatus;

    private void OnValidate()
    {
        _input = transform.GetComponentDebug<PlayerInput>();
        chrConPlayer = transform.GetComponentDebug<CharacterController>();
        _playerStatus = transform.GetComponentDebug<PlayerStatus>();
    }

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void LateUpdate()
    {
        RotateCharacter();
    }


    private void Move()
    {
        chrConPlayer.Move(_input.PlayerMoveDir.x * transform.right + _input.PlayerMoveDir.y * transform.forward);
    }

    //플레이어를 회전시킴
    private void RotateCharacter()
    {
        transform.eulerAngles += _input.MousePositionDir.x * _playerStatus.Sensitivity * Vector3.up;
    }
}
