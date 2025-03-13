using UnityEngine;

public class CharacterManager : MonoBehaviour //�̱��� ���� ĳ���� ��������
{
    private static CharacterManager _instance; //static ����
    public static CharacterManager Instance //�̱��� �ν��Ͻ� �����ü��ִ� ������Ƽ
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

    public Player Player //�÷��̾� ���� ����
    {
        get { return _player; }
        set { _player = value; }
    }
    private Player _player;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (_instance != this) //�ߺ� ��ü ����
            {
                Destroy(gameObject);
            }
        }
    }
}

