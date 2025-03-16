using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class Util
{
    private static readonly System.Random rand = new System.Random();
    /// <summary>
    /// 해당 오브젝트의 모든 자식들을 DFS 방식으로 재귀하며 같은 이름의 오브젝트를 반환하는 메서드
    /// </summary>
    /// <param name="parent">기준을 잡을 부모 오브젝트</param>
    /// <param name="name">찾을 이름</param>
    /// <returns>찾았다면 해당 오브젝트를, 실패하면 null반환</returns>
    public static Transform GetGameObjectSameNameDFS(this Transform parent, string name)
    {
        if (parent.name == name) return parent;

        for (int i = 0; i < parent.childCount; i++)
        {
            Transform tempTransform = parent.GetChild(i).GetGameObjectSameNameDFS(name);
            if (tempTransform != null) return tempTransform;
        }

        return null;
    }


    /// <summary>
    /// 해당 이름의 오브젝트를 찾고 컴포넌트를 반환하는 메서드
    /// </summary>
    /// <typeparam name="T">찾을 컴포넌트의 이름</typeparam>
    /// <param name="name">찾을 오브젝트의 이름</param>
    /// <param name="isDebug">Debug의 실행 유무, 기본값은 true</param>
    /// <param name="SearchDisable">비활성화 오브젝트를 찾을 지에 대한 유무, 기본값은 true</param>
    /// <returns>컴포넌트를 찾으면 반환, 못찾으면 null이다</returns>
    public static T GetComponentNameDFS<T>(this string name, bool isDebug = true, bool SearchDisable = true) where T : Component
    {
        Transform findTransform = GameObject.Find(name)?.transform;
        if(findTransform == null)
        {
            if(isDebug) Debug.LogError($"{name} is null");
            return null;
        }

        T tempT = findTransform.GetComponentInChildren<T>(SearchDisable);
        if (tempT == null && isDebug) Debug.LogError($"{typeof(T)} is null");
        return tempT;
    }


    /// <summary>
    /// FindFirstObjectByType와 똑같지만 자동으로 디버깅을 출력해준다.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="isDebug"></param>
    /// <returns></returns>
    public static T FindFirstObjectByTypeDebug<T>(bool isDebug = true) where T : Component
    {
        T tempT = Object.FindFirstObjectByType<T>();
        if (tempT == null && isDebug) Debug.LogError($"{typeof(T)} is null");
        return tempT;
    }


    /// <summary>
    /// 특정 트랜스폼 밑의 오브젝트 중 원하는 이름의 오브젝트에서 컴포넌트를 찾아 반환하는 메서드
    /// </summary>
    /// <typeparam name="T">찾고 싶은 컴포넌트</typeparam>
    /// <param name="parent">찾기 시작할 부모 오브젝트</param>
    /// <param name="name">부모 오브젝트에서 찾을 이름</param>
    /// <param name="isDebug">디버깅 유무, 기본값은 출력</param>
    /// <param name="SearchDisable">비활성화 오브젝트 탐색 여부, 기본값은 탐색</param>
    /// <returns></returns>
    public static T GetComponentForTransformFindName<T>(this Transform parent, string name, bool isDebug = true, bool SearchDisable = true) where T : Component
    {
        Transform transform = parent.GetGameObjectSameNameDFS(name);
        if(transform == null)
        {
            if (isDebug) Debug.LogError($"{name} is null");
            return null;
        }

        T tempT = transform.GetComponentInChildren<T>(SearchDisable);
        if (tempT == null && isDebug) Debug.LogError($"{typeof(T)} is null");
        return tempT;
    }


    /// <summary>
    /// 기존 GetComponent에서 Debug만 추가한 메서드 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="parent"></param>
    /// <param name="isDebug"></param>
    /// <returns></returns>
    public static T GetComponentDebug<T>(this Transform parent, bool isDebug = true)
    {
        T tempT = parent.GetComponent<T>();
        if (tempT == null && isDebug) Debug.LogError($"{typeof(T)} is null");
        return tempT;
    }


    /// <summary>
    /// 해당 Transform의 자식들을 검사해서 특정 컴포넌트를 가져오는 메서드
    /// </summary>
    /// <typeparam name="T">가져오고 싶은 컴포넌트</typeparam>
    /// <param name="parent">검사할 부모 오브젝트</param>
    /// <param name="isDebug">디버그 유무. 기본 값은 출력</param>
    /// <returns>있다면 반환. 없다면 null반환</returns>
    public static T GetChildComponentDebug<T>(this Transform parent, bool isDebug = true ) where T : Component
    {
        T tempT = parent.GetComponentInChildren<T>();
        if (tempT == null && isDebug) Debug.LogError($"{typeof(T)} is null");
        return tempT;
    }


    /// <summary>
    ///  GetcomponentInparent와 똑같지만 Debug만 추가됨.
    /// </summary>
    /// <typeparam name="T">가져오고 싶은 컴포넌트</typeparam>
    /// <param name="parent">찾기를 시작할 Transform</param>
    /// <param name="isDebug">디버그 출력 유무, 기본값은 출력</param>
    /// <returns></returns>
    public static T GetComponentInparentDebug<T>(this Transform parent, bool isDebug = true) where T : Component
    {
        T tempT = parent.GetComponentInParent<T>();
        if (tempT == null && isDebug) Debug.Log($"{typeof(T)} is null");
        return tempT;
    }


    /// <summary>
    /// 해당 실수를 더한 후 값을 제한하여 반환합니다.
    /// </summary>
    /// <param name="nowValue">현재 값</param>
    /// <param name="addValue">추가할 값</param>
    /// <param name="min">최소값</param>
    /// <param name="max">최대값</param>
    /// <returns>min <= nowValue + addValue <= min 반환</returns>
    public static float PlusAndClamp(this float nowValue, float addValue, float min, float max)
    {
        return Mathf.Clamp(nowValue + addValue, min, max);
    }
    /// <summary>
    /// 해당 실수를 더한 후 값을 제한하여 반환합니다.
    /// </summary>
    /// <param name="nowValue">현재 값</param>
    /// <param name="addValue">추가할 값</param>
    /// <param name="max">최대값</param>
    /// <returns>0 <= nowValue + addValue <= min 반환</returns>
    public static float PlusAndClamp(this float nowValue, float addValue, float max)
    {
        return Mathf.Clamp(nowValue + addValue, 0, max);
    }


    /// <summary>
    /// 리스트를 무작위로 섞는 메서드
    /// </summary>
    /// <typeparam name="T">리스트 내부에 넣을 요소</typeparam>
    /// <param name="list">섞을 리스트</param>
    public static void ShuffleList<T>(this List<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rand.Next(n + 1);
            (list[n], list[k]) = (list[k], list[n]);
        }
    }


    //커서 상태를 결정하는 메서드
    public static void CursorisLock(bool isLock)
    {
        if (isLock) Cursor.lockState = CursorLockMode.Locked;
        else Cursor.lockState = CursorLockMode.None;
    }
}
