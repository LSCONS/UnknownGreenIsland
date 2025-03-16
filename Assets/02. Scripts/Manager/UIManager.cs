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
                //해당 UIData를 복사해서 생성하고 연결함.
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
            //UIData에서 해당 Compunent가 들어있는 UIData를 가져옴.
            //해당 UIData에 있는 프리팹을 복사해서 생성함.
        }

        //IInitUI인터페이스를 상속하고 있다면 InitUI()를 실행.
        (tempT as IInitUI)?.InitUI();

        return tempT;
    }
}
