using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using VInspector;

public class PlayerControl : MonoBehaviour
{
    private PlayerInput input;
    private CharacterController chrConPlayer;
    private PlayerStatus playerStatus;
    private PlayerIsgrounded playerIsgrounded;
    private PlayerCheckAngle checkAngle;
    private float gravity = -9.81f;
    private Vector3 playerVelocity;
    [ShowInInspector]
    private float nowJumpForce = 0f;
    private float multiple = 15f;
    private float defaultGravity = -2;

    private Vector3 _difValue;



    private void OnValidate()
    {
        input = transform.GetComponentDebug<PlayerInput>();
        chrConPlayer = "PlayerObject".GetComponentNameDFS<CharacterController>();
        playerStatus = transform.GetComponentDebug<PlayerStatus>();
        _difValue = transform.position - chrConPlayer.transform.position;
        playerIsgrounded = "PlayerObject".GetComponentNameDFS<PlayerIsgrounded>();
        checkAngle = "PlayerObject".GetComponentNameDFS<PlayerCheckAngle>();
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
            chrConPlayer.transform.position + _difValue,
            Time.deltaTime * 40f
        );
    }


    //접지 상태일 때 Y의 Velocity를 defaultGravity로 초기화하는 메서드
    private void ResetYVelocityOnGround()
    {
        // 접지 상태일 때 y 속도 초기화
        if (playerIsgrounded.Isgrounded && playerVelocity.y <= 0)
        {
            playerVelocity.y = defaultGravity;
        }
    }


    //점프를 시도하는 메서드
    private void TryJump()
    {
        // 점프 입력 처리
        if (
                playerIsgrounded.Isgrounded &&
                input.IsJump && 
                !(input.IsInventory) &&
                nowJumpForce == 0f &&
                playerVelocity.y == defaultGravity &&
                playerStatus.CanJump()
            )
        {
            nowJumpForce = playerStatus.NewJumpForce;
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
        playerVelocity.y += (playerStatus.PlayerMass * gravity) * Time.fixedDeltaTime + nowJumpForce;
        //입력에 의해 움직이는 Move
        chrConPlayer.Move
        (
            input.PlayerMoveDir.x * playerStatus.MoveSpeed * Time.fixedDeltaTime * transform.right +
            playerVelocity.y * Time.fixedDeltaTime * transform.up +
            input.PlayerMoveDir.y * playerStatus.MoveSpeed * Time.fixedDeltaTime * transform.forward
        );
        //경로로 인해 밀어지는 힘을 주는 Move
        chrConPlayer.Move(checkAngle.repForce * Time.fixedDeltaTime);
    }


    //마우스 포지션에 따라 플레이어를 회전시키는 메서드
    private void RotateCharacter()
    {
        transform.eulerAngles += input.MousePositionDir.x * playerStatus.Sensitivity * Vector3.up;
    }
}
