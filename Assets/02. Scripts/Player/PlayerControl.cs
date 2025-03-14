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
        Util.CursorisLock(true);    //Ŀ�� ���¸� ��״� �޼���
    }


    private void FixedUpdate()
    {
        ResetYVelocityOnGround();   //���� ������ �� Y�� Velocity�� defaultGravity�� �ʱ�ȭ�ϴ� �޼���
        TryJump();                  //������ �õ��ϴ� �޼���
        Move();                     //�÷��̾ �̵���Ű�� �޼���
        ReduceJumpForce();          //������ �ϴ� ���� �����ִٸ� �ٿ��ִ� �޼���
    }


    private void Update()
    {
        RotateCharacter();          //���콺 �����ǿ� ���� �÷��̾ ȸ����Ű�� �޼���
    }


    private void LateUpdate()
    {
        FollowToCollider();         //�ݶ��̴��� ���� Lerp�� �����ϸ� �����̴� �޼���
    }


    //�ݶ��̴��� ���� Lerp�� �����ϸ� �����̴� �޼���
    private void FollowToCollider()
    {
        transform.position = Vector3.Lerp
        (
            transform.position,
            _chrConPlayer.transform.position + _difValue,
            Time.deltaTime * 40f
        );
    }


    //���� ������ �� Y�� Velocity�� defaultGravity�� �ʱ�ȭ�ϴ� �޼���
    private void ResetYVelocityOnGround()
    {
        // ���� ������ �� y �ӵ� �ʱ�ȭ
        if (_playerIsgrounded.Isgrounded && _playerVelocity.y <= 0)
        {
            _playerVelocity.y = defaultGravity;
        }
    }


    //������ �õ��ϴ� �޼���
    private void TryJump()
    {
        // ���� �Է� ó��
        if (_playerIsgrounded.Isgrounded && _input.IsJump && nowJumpForce == 0f && _playerVelocity.y == defaultGravity)
        {
            nowJumpForce = _playerStatus.NewJumpForce;
        }
    }


    //������ �ϴ� ���� �����ִٸ� �ٿ��ִ� �޼���
    private void ReduceJumpForce()
    {
        // ���� �� ���� ó��
        if (nowJumpForce > 0f)
        {
            nowJumpForce -= Time.fixedDeltaTime * multiple;
            if (nowJumpForce < 0f) nowJumpForce = 0f;
        }
    }


    //�÷��̾ �̵���Ű�� �޼���
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


    //���콺 �����ǿ� ���� �÷��̾ ȸ����Ű�� �޼���
    private void RotateCharacter()
    {
        transform.eulerAngles += _input.MousePositionDir.x * _playerStatus.Sensitivity * Vector3.up;
    }
}
