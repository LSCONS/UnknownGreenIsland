using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;
using VInspector;

[System.Flags]
public enum AbnormalStatus
{
    None = 0,               //상태이상 없음
    Bleeding = 1 << 0,      //출혈            //초당 데미지
    Poisoning = 1 << 1,     //중독            //초당 데미지
    Fracture = 1 << 2,      //골절            //달리기 금지            
    Dehydrration = 1 << 3,  //탈수            //달리기 불가능, 이동속도 감소
    Thirsty = 1 << 4,       //목마름          //스태미너 회복량 감소
    Drink = 1 << 5,         //물마심          //스태미너 회복량 증가
    PlentyWater = 1 << 6,   //수분 많음       //이동속도 증가, 최대 스태미너 증가
    Starvation = 1 << 7,    //아사            //공격력 감소, 최대 체력 감소
    Hunger = 1 << 8,        //배고픔          //초당 데미지
    Eat = 1 << 9,           //밥먹음          //초당 회복
    EatFull = 1 << 10,      //배부름          //최대 체력 증가, 공격력 버프
}

public enum PlayerAction
{
    Idle = 0,
    Move = 1,
    Run = 2,
    Jump = 3,
    Die = 4
}


public class PlayerStatus : MonoBehaviour
{
    #region 플레이어 기본 수치 선언
    private float moveSpeed = 5f;           //플레이어가 움직일 때 스피드 보정 값
    private float maxHealth = 100f;         //플레이어 최대 체력
    private float curHealth = 100f;         //플레이어 현재 체력
    private float maxStamina = 100f;        //플레이어 최대 스태미나
    private float curStamina = 100f;        //플레이어 현재 스태미나
    private float maxHunger = 100f;         //플레이어 최대 배고픔
    private float curHunger = 100f;         //플레이어 현재 배고픔
    private float maxThirsty = 100f;        //플레이어 최대 목마름
    private float curThirsty = 100f;        //플레이어 현재 목마름
    private float healthChangeValue = 0;    //플레이어의 체력 변화량
    private float staminaChangeValue = 3;   //플레이어의 스태미나 변화량
    private float damageValue = 5;          //플레이어 데미지 배수

    private AbnormalStatus curAbnormal = AbnormalStatus.None;    //플레이어 현재 상태이상

    [ShowInInspector]
    public float playerMass = 20f;          //플레이어 무게
    public float newJumpForce = 5f;         //플레이어 점프력

    //TODO: 추후에 해당 변수들 이동 필요 
    private float sensitivity = 0.1f;       //플레이어 마우스 감도
    private float maxCurXRot = 90;          //플레이어 시야 위 아래 최대 각도
    private float minCurXRot = -90f;        //플레이어 시야 위 아래 최소 각도
                                            //

    #endregion

    #region 플레이어 기본 수치 접근
    public float MoveSpeed { get => moveSpeed; }
    public float MaxHealth { get => maxHealth; }
    public float CurHealth { get => curHealth; }
    public float MaxStamina { get => maxStamina; }
    public float CurStamina { get => curStamina; }
    public float PlayerMass { get => playerMass; }
    public float MaxHunger { get => maxHunger; }
    public float CurHunger { get => curHunger; }
    public float MaxThirsty { get => maxThirsty; }
    public float CurThirsty { get => curThirsty; }
    public float NewJumpForce { get => newJumpForce; }
    public AbnormalStatus CurAbnormal { get => curAbnormal; }
    public float HealthChangeValue { get => healthChangeValue; }
    public float StaminaChangeValue { get => staminaChangeValue; }
    public float DamageValue { get => damageValue; }



    //TODO: 추후에 해당 변수들 이동 필요 
    public float Sensitivity { get => sensitivity; }
    public float MaxCurXRot { get => maxCurXRot; }
    public float MinCurXRot { get => minCurXRot; }
    //

    #endregion


    //체력의 값에 변동을 주는 메서드 
    public void HealthChange(float value)
    {
        curHealth = curHealth.PlusAndClamp(value, maxHealth);
        if (curHealth < 0)
        {
            //TODO: 사망처리 필요
        }
    }


    //스태미나의 값에 변동을 주는 메서드 
    public void StaminaChange(float value)
    {
        curStamina = curStamina.PlusAndClamp(value, maxStamina);
    }


    //배고픔 값에 변동을 주는 메서드
    public void HungerChange(float value)
    {
        curHunger = curHunger.PlusAndClamp(value, maxHunger);
        SetAbnormal(AbnormalStatus.Starvation, curHunger < 20f);
        SetAbnormal(AbnormalStatus.Hunger, curHunger < 40f);
        SetAbnormal(AbnormalStatus.Eat, curHunger >= 60f);
        SetAbnormal(AbnormalStatus.EatFull, curHunger >= 80f);
    }



    //목마름 값에 변동을 주는 메서드
    public void ThirstyChange(float value)
    {
        curThirsty = curThirsty.PlusAndClamp(value, maxThirsty);
        SetAbnormal(AbnormalStatus.Dehydrration, curHunger < 20f);
        SetAbnormal(AbnormalStatus.Thirsty, curHunger < 40f);
        SetAbnormal(AbnormalStatus.Drink, curHunger >= 60f);
        SetAbnormal(AbnormalStatus.PlentyWater, curHunger >= 80f);
    }


    //플레이어에게 특정 상태이상을 주는 메서드
    public void SetAbnormal(AbnormalStatus status, bool isSet)
    {
        AbnormalStatus prevAbnormal = curAbnormal;

        if (isSet) curAbnormal |= status;      //상태이상 추가
        else curAbnormal &= ~status;          //상태이상 제거

        if (prevAbnormal != curAbnormal)   //상태이상이 추가 또는 제거가 된 것이 확실하다면
        {
            ApplyAbnormalEffects(status, isSet);
        }
    }


    //바뀐 상태이상에 따라 효과를 적용시키는 메서드
    private void ApplyAbnormalEffects(AbnormalStatus state, bool isSet)
    {
        switch (state)
        {
            case AbnormalStatus.Bleeding:
                healthChangeValue += (isSet ? -1 : 1);
                break;

            case AbnormalStatus.Poisoning:
                healthChangeValue += (isSet ? -1 : 1);
                break;

            case AbnormalStatus.Fracture:
                //TODO: 달리기 금지 추가 필요 할지도?
                break;

            case AbnormalStatus.Dehydrration:
                //TODO: 달리기 금지 추가 필요 할지도?
                moveSpeed += (isSet ? -1 : 1);
                break;

            case AbnormalStatus.Thirsty:
                staminaChangeValue += (isSet ? -1 : 1);
                break;

            case AbnormalStatus.Drink:
                staminaChangeValue += (isSet ? 2 : -2);
                break;

            case AbnormalStatus.PlentyWater:
                maxStamina += (isSet ? 50 : -50);
                moveSpeed += (isSet ? 2 : -2);
                break;

            case AbnormalStatus.Starvation:
                damageValue += (isSet ? -2 : 2);
                maxHealth += (isSet ? -50 : 50);
                break;

            case AbnormalStatus.Hunger:
                healthChangeValue += (isSet ? -1 : 1);
                break;

            case AbnormalStatus.Eat:
                healthChangeValue += (isSet ? 1 : -1);
                break;

            case AbnormalStatus.EatFull:
                maxHealth += (isSet ? 50 : -50);
                damageValue += (isSet ? 2 : -2);
                break;

        }
    }


#if UNITY_EDITOR
    //효과 적용 테스트 에디터
    [Button]
    public void PlentyWaterOnOFF()
    {
        bool isPlently = (curAbnormal & AbnormalStatus.PlentyWater) == AbnormalStatus.PlentyWater;
        SetAbnormal(AbnormalStatus.PlentyWater, !isPlently);
        Debug.Log(!isPlently);
    }

    [Button]
    public void DehydrrationONOFF()
    {
        bool Dehydrration = (curAbnormal & AbnormalStatus.Dehydrration) == AbnormalStatus.Dehydrration;
        SetAbnormal(AbnormalStatus.Dehydrration, !Dehydrration);
        Debug.Log(!Dehydrration);
    }
#endif
}
