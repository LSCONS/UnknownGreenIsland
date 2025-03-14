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
    private PlayerIsgrounded _playerIsgrounded;
    private PlayerCheckAngle _checkAngle;
    private float _gravity = -9.81f;
    private Vector3 _playerVelocity;
    [ShowInInspector]
    private float nowJumpForce = 0f;
    private float multiple = 15f;
    private float defaultGravity = -2;

    private Vector3 _difValue;



    private void OnValidate()
    {
        _input = transform.GetComponentDebug<PlayerInput>();
        _chrConPlayer = "PlayerObject".GetComponentNameDFS<CharacterController>();
        _playerStatus = transform.GetComponentDebug<PlayerStatus>();
        _difValue = transform.position - _chrConPlayer.transform.position;
        _playerIsgrounded = "PlayerObject".GetComponentNameDFS<PlayerIsgrounded>();
        _checkAngle = "PlayerObject".GetComponentNameDFS<PlayerCheckAngle>();
    }


    private void Awake()
    {
        Util.CursorisLock(true);    //커서 상태를 잠그는 메서드
    }


    private void FixedUpdate()
    {
        ResetYVelocityOnGround();   //접지 상태일 때 Y의 Velocity를 defaultGravity로 초기화하는 메서드
        TryJump();                  //점프를 시도하는 메서드
        Move();                     //플레이어를 이동시키는 메서드
        ReduceJumpForce();          //점프를 하는 힘이 남아있다면 줄여주는 메서드
    }


    private void Update()
    {
        RotateCharacter();          //마우스 포지션에 따라 플레이어를 회전시키는 메서드
    }


    private void LateUpdate()
    {
        FollowToCollider();         //콜라이더를 따라 Lerp로 보간하며 움직이는 메서드
    }


    //콜라이더를 따라 Lerp로 보간하며 움직이는 메서드
    private void FollowToCollider()
    {
        transform.position = Vector3.Lerp
        (
            transform.position,
            _chrConPlayer.transform.position + _difValue,
            Time.deltaTime * 40f
        );
    }


    //접지 상태일 때 Y의 Velocity를 defaultGravity로 초기화하는 메서드
    private void ResetYVelocityOnGround()
    {
        // 접지 상태일 때 y 속도 초기화
        if (_playerIsgrounded.Isgrounded && _playerVelocity.y <= 0)
        {
            _playerVelocity.y = defaultGravity;
        }
    }


    //점프를 시도하는 메서드
    private void TryJump()
    {
        // 점프 입력 처리
        if (_playerIsgrounded.Isgrounded && _input.IsJump && nowJumpForce == 0f && _playerVelocity.y == defaultGravity)
        {
            nowJumpForce = _playerStatus.NewJumpForce;
        }
    }


    //점프를 하는 힘이 남아있다면 줄여주는 메서드
    private void ReduceJumpForce()
    {
        // 점프 힘 감소 처리
        if (nowJumpForce > 0f)
        {
            nowJumpForce -= Time.fixedDeltaTime * multiple;
            if (nowJumpForce < 0f) nowJumpForce = 0f;
        }
    }


    //플레이어를 이동시키는 메서드
    private void Move()
    {
        _playerVelocity.y += (_playerStatus.PlayerMass * _gravity) * Time.fixedDeltaTime + nowJumpForce;

        _chrConPlayer.Move(
            (_input.PlayerMoveDir.x * _playerStatus.MoveSpeed * Time.fixedDeltaTime * transform.right +
            _input.PlayerMoveDir.y * _playerStatus.MoveSpeed * Time.fixedDeltaTime * transform.forward +
            _playerVelocity.y * Time.fixedDeltaTime * transform.up
            ));
        _chrConPlayer.Move(_checkAngle.repForce * Time.fixedDeltaTime);
    }


    //마우스 포지션에 따라 플레이어를 회전시키는 메서드
    private void RotateCharacter()
    {
        transform.eulerAngles += _input.MousePositionDir.x * _playerStatus.Sensitivity * Vector3.up;
    }
}
