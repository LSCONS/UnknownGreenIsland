using UnityEngine;

public class CharacterManager : MonoBehaviour //싱글톤 패턴 캐릭터 정보관리
{
    private static CharacterManager _instance; //static 선언
    public static CharacterManager Instance //싱글톤 인스턴스 가져올수있는 프로퍼티
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject("CharacerManager").AddComponent<CharacterManager>();
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (_instance != this) //중복 객체 삭제
            {
                Destroy(gameObject);
            }
        }
    }
}

