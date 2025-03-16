using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VInspector;

public class ConditionHandler : MonoBehaviour, IInitUI
{
    [ReadOnly, ShowInInspector]
    private Condition conditionHP;
    [ReadOnly, ShowInInspector]
    private Condition conditionstamina;
    [ReadOnly, ShowInInspector]
    private Condition conditionHunger;
    [ReadOnly, ShowInInspector]
    private Condition conditionThirsty;

    public Condition ConditionHP { get => conditionHP; }
    public Condition Conditionstamina { get => conditionstamina; }
    public Condition ConditionHunger { get => conditionHunger; }
    public Condition ConditionThirsty { get => conditionThirsty; }

    private void OnValidate()
    {
        conditionHP = transform.GetComponentForTransformFindName<Condition>("ConditionHP");
        conditionstamina = transform.GetComponentForTransformFindName<Condition>("ConditionStamina");
        conditionHunger = transform.GetComponentForTransformFindName<Condition>("ConditionHunger");
        conditionThirsty = transform.GetComponentForTransformFindName<Condition>("ConditionThirst");
    }


    public void InitUI()
    {
        UIManager.Instance.ConditionHandler = this;
    }
}
