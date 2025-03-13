using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;
using VInspector;

public class PlayerStatus : MonoBehaviour
{
    private float moveSpeed = 5f;
    private float jumpForce = 5f;
    private float maxHealth = 100f;
    private float curHealth = 100f;
    private float maxStamina = 100f;
    private float curStamina = 100f;
    private float maxPositionY;
    private float staminaRecoverySpeed = 20f;
    [ShowInInspector]
    public float playerMass = 20f;
    public float newJumpForce = 5f;
    public float NewJumpForce { get => newJumpForce; }

    private LayerMask _excludeLayerMask;
    private CharacterController _chrcon;

    //TODO: 추후에 해당 변수들 이동 필요 
    private float sensitivity = 0.1f;
    private float maxCurXRot = 90;
    private float minCurXRot = -90f;
    //

    public float MoveSpeed { get => moveSpeed; }
    public float JumpForce { get => jumpForce; }
    public float MaxHealth { get => maxHealth; }
    public float CurHealth { get => curHealth; }
    public float MaxStamina { get => maxStamina; }
    public float CurStamina { get => curStamina; }
    public float PlayerMass { get => playerMass; }

    //TODO: 추후에 해당 변수들 이동 필요 
    public float Sensitivity { get => sensitivity; }
    public float MaxCurXRot { get => maxCurXRot; }
    public float MinCurXRot { get => minCurXRot; }
    //

    private void OnValidate()
    {
        _excludeLayerMask = ~(ReadonlyData.EnemyLayerMask | ReadonlyData.PlayerLayerMask);
        _chrcon = "Controller_Player".GetComponentNameDFS<CharacterController>();
    }

    //체력의 값에 변동을 주는 메서드 
    public void HealthChange(float value)
    {

    }


    //스태미나의 값에 변동을 주는 메서드 
    public void StaminaChange(float value)
    {

    }
}
