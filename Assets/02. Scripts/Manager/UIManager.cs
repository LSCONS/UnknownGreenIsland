using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    private ConditionHandler conditionHandler;
    public ConditionHandler ConditionHandler
    {
        get
        {
            if(conditionHandler == null)
            {
                //�ش� UIData�� �����ؼ� �����ϰ� ������.
            }
            return conditionHandler;
        }
        set
        {
            conditionHandler = value;
        }
    }


    private T TryGetComponentAndAdd<T>() where T : Component
    {
        T tempT = FindFirstObjectByType<T>();
        if(tempT == null)
        {
            //UIData���� �ش� Compunent�� ����ִ� UIData�� ������.
            //�ش� UIData�� �ִ� �������� �����ؼ� ������.
        }

        //IInitUI�������̽��� ����ϰ� �ִٸ� InitUI()�� ����.
        (tempT as IInitUI)?.InitUI();

        return tempT;
    }
}
