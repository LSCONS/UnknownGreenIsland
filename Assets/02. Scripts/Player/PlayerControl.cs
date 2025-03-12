using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using VInspector;

public class PlayerControl : MonoBehaviour
{
    private PlayerInput _input;
    private CharacterController _chrConPlayer;
    private PlayerStatus _playerStatus;
    private float _gravity = -9.81f;
    private Vector3 _playerVelocity;
    [ShowInInspector]
    private float nowJumpForce = 0f;
    public float multiple = 0f;
    private float NowJumpForce { get => nowJumpForce; }     //현재 받고 있는 점프의 힘을 저장하는 변수

    private float playerSkinWidth;

    private float playerMaxY = 0;
    private bool isOut = false;

    private void OnValidate()
    {
        _input = transform.GetComponentDebug<PlayerInput>();
        _chrConPlayer = transform.GetChildComponentDebug<CharacterController>();
        _playerStatus = transform.GetComponentDebug<PlayerStatus>();
    }

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        _playerStatus.CheckIsGround();
        if (_playerStatus.IsGround && _playerVelocity.y <= 0) _playerVelocity.y = 0f;
        if (_playerStatus.IsGround && _input.IsJump && nowJumpForce == 0f && _playerVelocity.y == 0f)
        {
            nowJumpForce = _playerStatus.NewJumpForce;
        }
        Move();
        if (nowJumpForce > 0f)
        {
            nowJumpForce -= Time.fixedDeltaTime * multiple;
            if (nowJumpForce < 0f) nowJumpForce = 0f;
        }
    }


    private void Update()
    {
        RotateCharacter();

        if (!(_playerStatus.IsGround))
        {
            playerMaxY = Mathf.Max(playerMaxY, transform.position.y);
            isOut = true;
        }
        else
        {
            if(isOut)Debug.Log(playerMaxY);
            isOut =false;
            playerMaxY = 0f;
        }
    }


    private void Move()
    {
        _playerVelocity.y += (_playerStatus.PlayerMass *_gravity) * Time.fixedDeltaTime + nowJumpForce;

        _chrConPlayer.Move(
            _input.PlayerMoveDir.x * Time.fixedDeltaTime * transform.right +
            _input.PlayerMoveDir.y * Time.fixedDeltaTime * transform.forward +
            _playerVelocity.y * Time.fixedDeltaTime * transform.up
            );
    }

    //플레이어를 회전시킴
    private void RotateCharacter()
    {
        transform.eulerAngles += _input.MousePositionDir.x * _playerStatus.Sensitivity * Vector3.up;
    }

}
