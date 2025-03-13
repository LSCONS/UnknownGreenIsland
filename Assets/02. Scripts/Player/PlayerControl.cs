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
    public float multiple = 0f;
    public float defaultGravity;
    private float NowJumpForce { get => nowJumpForce; }     //���� �ް� �ִ� ������ ���� �����ϴ� ����

    private Vector3 _difValue;

    private float playerMaxY = 0;
    private bool isOut = false;

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
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {       
        // ���� ������ �� y �ӵ� �ʱ�ȭ
        if (_playerIsgrounded.Isgrounded && _playerVelocity.y <= 0)
        {
            _playerVelocity.y = -defaultGravity;
        }

        // ���� �Է� ó��
        if (_playerIsgrounded.Isgrounded && _input.IsJump && nowJumpForce == 0f && _playerVelocity.y == -defaultGravity)
        {
            nowJumpForce = _playerStatus.NewJumpForce;
        }

        Move();

        // ���� �� ���� ó��
        if (nowJumpForce > 0f)
        {
            nowJumpForce -= Time.fixedDeltaTime * multiple;
            if (nowJumpForce < 0f) nowJumpForce = 0f;
        }
    }


    private void Update()
    {
        if (!(_playerIsgrounded.Isgrounded))
        {
            playerMaxY = Mathf.Max(playerMaxY, transform.position.y);
            isOut = true;
        }
        else
        {
            if (isOut) Debug.Log(playerMaxY);
            isOut = false;
            playerMaxY = 0f;
        }

        RotateCharacter();
    }

    private void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, _chrConPlayer.transform.position + _difValue, Time.deltaTime * 20);
    }


    private void Move()
    {
        _playerVelocity.y += (_playerStatus.PlayerMass *_gravity) * Time.fixedDeltaTime + nowJumpForce;

        _chrConPlayer.Move(
            (_input.PlayerMoveDir.x * Time.fixedDeltaTime * transform.right +
            _input.PlayerMoveDir.y * Time.fixedDeltaTime * transform.forward +
            _playerVelocity.y * Time.fixedDeltaTime * transform.up
            )+ _checkAngle.repForce * Time.fixedDeltaTime);
    }

    //�÷��̾ ȸ����Ŵ
    private void RotateCharacter()
    {
        transform.eulerAngles += _input.MousePositionDir.x * _playerStatus.Sensitivity * Vector3.up;
    }

}
