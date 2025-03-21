using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerInput : MonoBehaviour
{
    private PlayerInputSystem inputSystem;
    private PlayerStatus playerStatus;
    private PlayerInteraction playerInteraction;
    private Animator playerAnimator;

    private Vector2 playerMoveDir;
    public Vector2 PlayerMoveDir { get => playerMoveDir; }

    private Vector2 mousePositionDir;
    public Vector2 MousePositionDir { get => mousePositionDir; }
    private bool isJump = false;
    public bool IsJump { get =>  isJump; }
    private bool isRun = false;
    public bool IsRun { get => isRun; }
    private bool isInventory = false;
    public bool IsInventory { get => isInventory; }

    public Action inventoryAction;
    public Action interactionAction;
    public Action inventoryExitAction;
    public Action<string, bool> Attack;

    private void OnValidate()
    {
        playerStatus = transform.GetComponentDebug<PlayerStatus>();
        playerInteraction = transform.GetComponentDebug<PlayerInteraction>();
        playerAnimator = transform.GetComponentForTransformFindName<Animator>("Character");
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
        inputSystem.Player.Run.started += OnRun;
        inputSystem.Player.Run.canceled += StopRun;
        inputSystem.Player.Inventory.started += ToggleInventory;
        inputSystem.Player.Interaction.started += InteractionStart;

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
        inputSystem.Player.Run.started -= OnRun;
        inputSystem.Player.Run.canceled -= StopRun;
        inputSystem.Player.Inventory.started -= ToggleInventory;
        inputSystem.Player.Interaction.started -= InteractionStart;

        inputSystem.Disable();
    }


    //TODO: 플레이어가 인벤토리를 킨 상태로 WASD 입력을 한 상태로 false로 갈 경우 입력 값이 남아있을 수 있도록 변환.
    private void OnMove(InputAction.CallbackContext context)
    {
        if (!(IsInventory))
        {
            playerMoveDir = context.ReadValue<Vector2>().normalized;
            playerAnimator.SetFloat("Blend",0.2f);
        }
        else { playerMoveDir = Vector2.zero; }
    }
    private void StopMove(InputAction.CallbackContext context)
    {
        playerMoveDir = Vector2.zero;
        playerAnimator.SetFloat("Blend",0f);
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
        Attack?.Invoke(ReadonlyAnimator.Attack, true);
    }
    private void StopAction(InputAction.CallbackContext context)
    {
        Attack?.Invoke(ReadonlyAnimator.Attack, false);
    }
    private void OnRun(InputAction.CallbackContext context)
    {
        isRun = true;
        playerAnimator.SetFloat("Blend", 0.4f);
        playerStatus.CanRun();
    }
    private void StopRun(InputAction.CallbackContext context)
    {
        isRun = false;
        playerAnimator.SetFloat("Blend", 0);
    }
    private void InteractionStart(InputAction.CallbackContext context)
    {
        if (!(IsInventory))
        {
            interactionAction?.Invoke();
        }
    }
    private void ToggleInventory(InputAction.CallbackContext context)
    {
        IsInventoryToggle();
        if (IsInventory) 
        {
            playerMoveDir = Vector2.zero;
            inventoryAction?.Invoke();
        }
        else
        {
            inventoryExitAction?.Invoke();
            Util.CursorisLock(!(IsInventory));
        }
    }


    //인벤토리의 현재 상태를 바꿀 때 접근하는 메서드
    public void IsInventoryToggle()
    {
        isInventory = !isInventory;
    }
}
