using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



public enum ItemType
{
    Equipable, //장착 아이템
    Consumable, //소비 아이템
    Resource, //자원 아이템
}


public enum ConsumableType
{
    Health, //체력
    Stamina, //스태미나
    Hunger, //배고픔
    Thirsty //목마름
}


//재료가 될 수 있는 아이템들을 정리
public enum Resource
{
    None, //설정 없음
    Wood, //나무조각 아이템
    Stone, //돌 아이템
    LogWood, //통나무 아이템
    Leather, //가죽 아이템
    Fiber, //식물섬유
    Obsidian, //흑요석
    BlueFree, //푸른광물
    IronStone, //철광석

    RawMeat, //생고기
    GreenMeat, //고블린 생고기
    Apple, //사과
    GreenApple, //초록 사과
    CaveMushroom, //동굴버섯
    Carrot, //당근

    GreenTeaLeaves, //녹잎
    Honey, //꿀
    Herbal, //허브
    YellowHerb, //황련
    JachoLeaf, //자초
    BaekBan, //백반

    CleanWater, //깨끗한 물
    HerbalTea, //허브차


}


[Serializable]
public class CrafitingResource
{
    public Resource type; //조합하는데 필요한 아이템 재료
    public float Amount; //해당 아이템의 수
}


[Serializable]
public class ItemDataConsumabale
{
    public ConsumableType type; //타입 설정
    public float value; //아이템 사용 후 증가 값
}

[CreateAssetMenu(fileName = "Item", menuName = "New Item")]
public class ItemData : ScriptableObject
{
    [Header("Info")]
    public int ItemID; //아이템 번호
    public string ItemName; //아이템 이름
    public string description; //아이템 설명
    public string interaction_Information;  //상호작용 정보
    public AbnormalStatus abnormalStatus; //디버프 설정
    public Resource resourceType;       //해당 아이템의 재료 타입
    public ItemType type; //아이템 타입
    public Sprite inventory_icon; //인벤토리 아이콘
    public Sprite interaction_Icon; //상호작용 아이콘
    public GameObject dropPrefab; //드롭했을 때 나타나는 3D 오브젝트

    [Header("QuantityLimit")]
    public bool canStack; //다중 보유가능 유무 true= 다중, false=한개만
    public int maxStackAmount; //최대 보유수량

    [Header("Consumable")]
    public ItemDataConsumabale[] consumabale; //아이템 효과값 배열로 만듬 
    public CrafitingResource[] resources; //조합하기 위한 재료 아이템들

    public class Consumable
    {
        public ConsumableType type;  // 회복 종류 (배고픔, 체력 등)
        public float value;          // 회복량
    }
}

