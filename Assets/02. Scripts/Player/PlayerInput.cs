using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    PlayerInputSystem playerinput;
    WeaponHandler weaponHandler;

    public float moveSpeed = 10f;


    private Vector2 playerMoveDir;
    public Vector2 PlayerMoveDir { get => playerMoveDir; }

    private Vector2 mousePositionDir;
    public Vector2 MousePositionDir { get => mousePositionDir; }
    private bool isJump = false;
    public bool IsJump { get =>  isJump; }


    private void Awake()
    {
        weaponHandler = "WeaponPivot".GetComponentNameDFS<WeaponHandler>();
    }

    private void OnEnable()
    {
        playerinput = new PlayerInputSystem();

        playerinput.Player.Move.performed += OnMove;
        playerinput.Player.Move.canceled += StopMove;
        playerinput.Player.MousePosition.performed += OnMousePosition;
        playerinput.Player.MousePosition.canceled += StopMousePosition;
        playerinput.Player.Jump.started += OnJump;
        playerinput.Player.Jump.canceled += StopJump;
        playerinput.Player.Action.started += OnAction; 
        playerinput.Player.Action.canceled += StopAction;


        playerinput.Enable();
    }


    private void OnDisable()
    {
        playerinput.Player.Move.performed -= OnMove;
        playerinput.Player.Move.canceled -= StopMove;
        playerinput.Player.MousePosition.performed -= OnMousePosition;
        playerinput.Player.MousePosition.canceled -= StopMousePosition;
        playerinput.Player.Jump.started -= OnJump;
        playerinput.Player.Jump.canceled -= StopJump;
        playerinput.Player.Action.started -= OnAction;
        playerinput.Player.Action.canceled -= StopAction;



        playerinput.Disable();
    }


    private void OnMove(InputAction.CallbackContext context)
    {
        playerMoveDir = context.ReadValue<Vector2>().normalized * moveSpeed;
    }

    private void StopMove(InputAction.CallbackContext context)
    {
        playerMoveDir = Vector2.zero;
    }

    private void OnMousePosition(InputAction.CallbackContext context)
    {
        mousePositionDir = context.ReadValue<Vector2>();
    }

    private void StopMousePosition(InputAction.CallbackContext context)
    {
        mousePositionDir = Vector2.zero;
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        isJump = true;
    }

    private void StopJump(InputAction.CallbackContext context)
    {
        isJump = false;
    }

    private void OnAction(InputAction.CallbackContext context) 
    {
        weaponHandler.Attack();
    }
    private void StopAction(InputAction.CallbackContext context)
    {
        weaponHandler.StopAttack();

    }
}
