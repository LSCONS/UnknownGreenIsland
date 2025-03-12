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
    private bool isGround = false;
    private float playerMass = 3;
    private float newJumpForce = 0.7f;
    public float NewJumpForce { get => newJumpForce; }

    private LayerMask _excludeLayerMask;

    //TODO: 추후에 해당 변수들 이동 필요 
    private float sensitivity = 0.1f;
    private float maxCurXRot = 90;
    private float minCurXRot = -90f;
    private float consumptionJump = 20f;
    //

    public float MoveSpeed { get => moveSpeed; }
    public float JumpForce { get => jumpForce; }
    public float MaxHealth { get => maxHealth; }
    public float CurHealth { get => curHealth; }
    public float MaxStamina { get => maxStamina; }
    public float CurStamina { get => curStamina; }
    public bool IsGround { get => isGround; }
    public float PlayerMass { get => playerMass; }

    //TODO: 추후에 해당 변수들 이동 필요 
    public float Sensitivity { get => sensitivity; }
    public float MaxCurXRot { get => maxCurXRot; }
    public float MinCurXRot { get => minCurXRot; }
    //


    //체력의 값에 변동을 주는 메서드 
    public void HealthChange(float value)
    {

    }


    //스태미나의 값에 변동을 주는 메서드 
    public void StaminaChange(float value)
    {

    }


    //플레이어가 땅에 닿고 있는지 확인하고 반환하는 메서드 
    public bool CheckIsGround()
    {
        Ray[] ray = new Ray[]
        {
            new Ray(transform.position + Vector3.forward * 0.3f + Vector3.up * 0.01f, Vector3.down),
            new Ray(transform.position + Vector3.back * 0.3f+ Vector3.up * 0.01f, Vector3.down),
            new Ray(transform.position + Vector3.right * 0.3f+ Vector3.up * 0.01f, Vector3.down),
            new Ray(transform.position + Vector3.left * 0.3f+ Vector3.up * 0.01f, Vector3.down)
        };

        for (int i = 0; i < ray.Length; i++)
        {
            if (Physics.Raycast(ray[i], 0.02f, _excludeLayerMask))
            {
                return true;
            }
        }
        return false;
    }
}
