using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    private PlayerInputSystem inputSystem;
    private WeaponHandler weaponHandler;
    private Animator animator;
    private PlayerStatus playerStatus;

    private Vector2 playerMoveDir;
    public Vector2 PlayerMoveDir { get => playerMoveDir; }

    private Vector2 mousePositionDir;
    public Vector2 MousePositionDir { get => mousePositionDir; }
    private bool isJump = false;
    public bool IsJump { get =>  isJump; }
    private bool isRun = false;
    public bool IsRun { get => isRun; }

    private void OnValidate()
    {
        playerStatus = transform.GetComponentDebug<PlayerStatus>();
    }


    private void Awake()
    {
        weaponHandler = "WeaponPivot".GetComponentNameDFS<WeaponHandler>();
        animator = weaponHandler.GetComponent<Animator>();
    }

    private void OnEnable()
    {
        inputSystem = new PlayerInputSystem();

        inputSystem.Player.Move.performed += OnMove;
        inputSystem.Player.Move.canceled += StopMove;
        inputSystem.Player.MousePosition.performed += OnMousePosition;
        inputSystem.Player.MousePosition.canceled += StopMousePosition;
        inputSystem.Player.Jump.started += OnJump;
        inputSystem.Player.Jump.canceled += StopJump;
        inputSystem.Player.Action.started += OnAction;
        inputSystem.Player.Action.canceled += StopAction;


        inputSystem.Enable();
    }


    private void OnDisable()
    {
        inputSystem.Player.Move.performed -= OnMove;
        inputSystem.Player.Move.canceled -= StopMove;
        inputSystem.Player.MousePosition.performed -= OnMousePosition;
        inputSystem.Player.MousePosition.canceled -= StopMousePosition;
        inputSystem.Player.Jump.started -= OnJump;
        inputSystem.Player.Jump.canceled -= StopJump;
        inputSystem.Player.Action.started -= OnAction;
        inputSystem.Player.Action.canceled -= StopAction;


        inputSystem.Disable();
    }


    private void OnMove(InputAction.CallbackContext context)
    {
        playerMoveDir = context.ReadValue<Vector2>().normalized;
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
        animator.SetBool("IsAttack", true);
    }
    private void StopAction(InputAction.CallbackContext context)
    {
        animator.SetBool("IsAttack", false);
    }
    private void OnRun(InputAction.CallbackContext context)
    {
        
    }
    private void StopRun(InputAction.CallbackContext context)
    {
        isRun = false;
    }
}
