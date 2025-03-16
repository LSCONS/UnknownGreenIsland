using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
    [ShowInInspector]
    private float moveSpeed = 5f;           //플레이어가 움직일 때 스피드 보정 값
    private float runMultiple = 1f;         //플레이어가 달릴 때 스피드 곱
    private float maxHealth = 100f;         //플레이어 최대 체력
    private float curHealth = 100f;         //플레이어 현재 체력
    private float maxStamina = 100f;        //플레이어 최대 스태미나
    private float curStamina = 100f;        //플레이어 현재 스태미나
    private float maxHunger = 100f;         //플레이어 최대 배고픔
    private float curHunger = 100f;         //플레이어 현재 배고픔
    private float maxThirsty = 100f;        //플레이어 최대 목마름
    private float curThirsty = 100f;        //플레이어 현재 목마름
    private int healthChangeValue = 0;    //플레이어의 초당 체력 변화량
    private int staminaChangeValue = 3;   //플레이어의 초당 스태미나 변화량
    private float damageValue = 5;          //플레이어 데미지 배수
    private Dictionary<AbnormalStatus, int> abnormalTimers = new Dictionary<AbnormalStatus, int>();

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
    public float MoveSpeed { get => moveSpeed * runMultiple; }
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
    public int HealthChangeValue { get => healthChangeValue; }
    public int StaminaChangeValue { get => staminaChangeValue; }
    public float DamageValue { get => damageValue; }
    public Dictionary<AbnormalStatus, int> AbnormalTimers { get => abnormalTimers; }



    //TODO: 추후에 해당 변수들 이동 필요 
    public float Sensitivity { get => sensitivity; }
    public float MaxCurXRot { get => maxCurXRot; }
    public float MinCurXRot { get => minCurXRot; }
    //

    #endregion

    private PlayerInput playerInput;
    private ConditionHandler conditionHandler;
    private List<AbnormalStatus> removeStateKey = new List<AbnormalStatus>();
    private List<AbnormalStatus> copyStateKey = new List<AbnormalStatus>();
    private WaitForSeconds abnormalWait = new WaitForSeconds(0.1f);
    private Coroutine abnormalCoroutine = null;
    private Coroutine runCoroutine = null;
    private AbnormalStatus cantRunAbnormal =
        (AbnormalStatus.Fracture | AbnormalStatus.Dehydrration);//달릴 수 없는 상태이상들

    private void OnValidate()
    {
        playerInput = transform.GetComponentDebug<PlayerInput>();
        conditionHandler = transform.GetComponentForTransformFindName<ConditionHandler>("UI_Condition_Canvas");
    }

    private void Start()
    {
        abnormalCoroutine = StartCoroutine(AbnormalCoroutine());
    }


    //체력의 값에 변동을 주는 메서드 
    public void HealthChange(float value)
    {
        curHealth = curHealth.PlusAndClamp(value, maxHealth);
        conditionHandler.ConditionHP.UpdateBar(curHealth / maxHealth);

        if (curHealth <= 0)
        {
            //TODO: 사망처리 필요
        }
    }


    //스태미나의 값에 변동을 주는 메서드 
    public void StaminaChange(float value)
    {
        curStamina = curStamina.PlusAndClamp(value, maxStamina);
        conditionHandler.Conditionstamina.UpdateBar(curStamina / maxStamina);
    }


    //배고픔 값에 변동을 주는 메서드
    public void HungerChange(float value)
    {
        curHunger = curHunger.PlusAndClamp(value, maxHunger);
        SetAbnormal(AbnormalStatus.Starvation, curHunger < 20f);
        SetAbnormal(AbnormalStatus.Hunger, curHunger < 40f);
        SetAbnormal(AbnormalStatus.Eat, curHunger >= 60f);
        SetAbnormal(AbnormalStatus.EatFull, curHunger >= 80f);
        conditionHandler.ConditionHunger.UpdateBar(curHunger / maxHunger);
    }


    //목마름 값에 변동을 주는 메서드
    public void ThirstyChange(float value)
    {
        curThirsty = curThirsty.PlusAndClamp(value, maxThirsty);
        SetAbnormal(AbnormalStatus.Dehydrration, curThirsty < 20f);
        SetAbnormal(AbnormalStatus.Thirsty, curThirsty < 40f);
        SetAbnormal(AbnormalStatus.Drink, curThirsty >= 60f);
        SetAbnormal(AbnormalStatus.PlentyWater, curThirsty >= 80f);
        conditionHandler.ConditionThirsty.UpdateBar(curThirsty / maxThirsty);
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
                break;

            case AbnormalStatus.Dehydrration:
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


    /// <summary>
    ///플레이어에게 특정 상태이상을 주는 메서드
    /// </summary>
    /// <param name="status">설정할 상태이상</param>
    /// <param name="isSet">설정할지 해제할지 결정</param>
    public void SetAbnormal(AbnormalStatus status, bool isSet)
    {
        AbnormalStatus prevAbnormal = curAbnormal;

        if (isSet) curAbnormal |= status;      //상태이상 추가
        else curAbnormal &= ~status;          //상태이상 제거

        if (prevAbnormal != curAbnormal)   //상태이상이 추가 또는 제거가 된 것이 확실하다면 효과 적용
        {
            ApplyAbnormalEffects(status, isSet);
            //TODO: 상태이상 UI 갱신 필요
        }
    }


    /// <summary>
    /// 해당 메서드를 통해 특정 상태이상과 지속 시간을 동시에 추가할 수 있음.
    /// </summary>
    /// <param name="state">추가할 상태이상</param>
    /// <param name="time">해당 상태이상의 시간, 1당 0.1초의 지속시간</param>
    public void AddAbnormalTimer(AbnormalStatus state, int time)
    {
        if (abnormalTimers.ContainsKey(state))
        {
            abnormalTimers[state] += time;
        }
        else        //새로 생성된 경우라면 해당 상태이상을 true로 추가함.
        {
            abnormalTimers.Add(state, time);
            SetAbnormal(state, true);
        }
    }


    /// <summary>
    /// 아이템을 사용해서 지속시간을 없애는 경우 해당 메서드로 호출.
    /// </summary>
    /// <param name="state">삭제시킬 상태이상</param>
    public void RemoveAbnormalTimer(AbnormalStatus state)
    {
        abnormalTimers.Remove(state);
        SetAbnormal(state, false);
    }


    //상태이상의 효과를 적용시키는 메서드
    private void ApplyAbnormal()
    {
        //체력, 스태미나 변화값이 0이 아닌경우 효과 적용
        if (healthChangeValue != 0) HealthChange(0.1f * healthChangeValue);
        if (staminaChangeValue != 0) StaminaChange(0.1f * staminaChangeValue);

        //현재 상태이상 리스트가 존재할 경우
        if (abnormalTimers.Count > 0)
        {
            copyStateKey.Clear();
            copyStateKey.AddRange(abnormalTimers.Keys);
            removeStateKey.Clear();

            foreach (AbnormalStatus state in copyStateKey)
            {
                abnormalTimers[state] -= 1;
                if (abnormalTimers[state] == 0) removeStateKey.Add(state);
            }

            foreach (AbnormalStatus state in removeStateKey)
            {
                RemoveAbnormalTimer(state);
            }
        }
    }


    //상태이상의 효과를 0.1초마다 줄 메서드
    IEnumerator AbnormalCoroutine()
    {
        while (true)
        {
            ApplyAbnormal();
            yield return abnormalWait;
        }
    }


    //플레이어가 달릴 수 있는 상태인지 확인하고 스태미나를 깎는 메서드
    public bool CanRun()
    {
        //해당 메서드는 1회만 호출 되어야함.
        //일단 상태이상으로 달릴 수 없는 상태이상인지 확인해야함,
        //플레이어가 방향키의 입력으로 받고 있는 방향벡터가 있는지 확인해야함.
        //플레이어가 사용할 수 있는 스태미나가 있는지 확인해야함.
        //플레이어가 땅에 붙어있는 상태여야함.
        if (playerInput.IsRun &&
            curAbnormal == (curAbnormal & ~cantRunAbnormal) &&
            playerInput.PlayerMoveDir != Vector2.zero &&
            curStamina >= 1f)
        {
            Debug.Log("플레이어 달리는 중");
            StaminaChange(-1f);
            if (runCoroutine == null) runCoroutine = StartCoroutine(RunCoroutine());
            return true;
        }
        else
        {
            return false;
        }
    }


    //플레이어가 점프할 수 있는 상태인지 확인하고 스태미나를 깎는 메서드
    public bool CanJump()
    {
        if (curStamina >= 10f)
        {
            StaminaChange(-10f);
            return true;
        }
        else return false;
    }


    //특정 시간동안 플레이어의 속도를 높여주는 코루틴
    private IEnumerator RunCoroutine()
    {
        runMultiple = 2;
        while (true)
        {
            yield return abnormalWait;
            if (!(CanRun())) break;
        }
        runMultiple = 1;
        runCoroutine = null;
    }


#if UNITY_EDITOR
    //효과 적용 테스트 에디터
    [Button]
    public void PlentyWaterOnOFF()
    {
        Debug.Log("상태이상 추가");
        AddAbnormalTimer(AbnormalStatus.PlentyWater, 50);
        Debug.Log(abnormalTimers[AbnormalStatus.PlentyWater]);
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
