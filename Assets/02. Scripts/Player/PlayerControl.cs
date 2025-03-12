using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    private PlayerInput _input;
    private CharacterController chrConPlayer;
    private PlayerStatus _playerStatus;
    private float _gravity = -9.81f;
    private Vector3 _playerVelocity;
    public bool isGrounded;
    private float nowJumpForce = 0f;
    private float NowJumpForce { get => nowJumpForce; }     //현재 받고 있는 점프의 힘을 저장하는 변수

    private float playerMaxY = 0;
    private bool isOut = false;

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

    private void Update()
    {
        isGrounded = chrConPlayer.isGrounded;
        if (isGrounded) _playerVelocity.y = -0.1f;

        if (isGrounded && _input.IsJump)
        {
            nowJumpForce = _playerStatus.NewJumpForce;
        }

        

        Move();
        RotateCharacter();

        if(nowJumpForce > 0f)
        {
            nowJumpForce -= Time.deltaTime * 5;
            if(nowJumpForce < 0f) nowJumpForce = 0f;
        }

        if (isGrounded == false)
        {
            playerMaxY = Mathf.Max(playerMaxY, transform.position.y);
            isOut = true;
        }
        else
        {
            if(isOut)Debug.Log(playerMaxY);
            isOut =false;
        }


    }


    private void Move()
    {
        _playerVelocity.y += (_playerStatus.PlayerMass *_gravity) * Time.deltaTime + nowJumpForce;

        chrConPlayer.Move(
            _input.PlayerMoveDir.x * Time.deltaTime * transform.right +
            _input.PlayerMoveDir.y * Time.deltaTime * transform.forward +
            _playerVelocity.y * Time.deltaTime * transform.up
            );
    }

    //플레이어를 회전시킴
    private void RotateCharacter()
    {
        transform.eulerAngles += _input.MousePositionDir.x * _playerStatus.Sensitivity * Vector3.up;
    }
}
