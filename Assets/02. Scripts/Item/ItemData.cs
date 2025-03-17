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
    public ItemType type; //아이템 타입
    public Sprite inventory_icon; //인벤토리 아이콘
    public Sprite interaction_Icon; //상호작용 아이콘
    public GameObject dropPrefab; //드롭했을 때 나타나는 3D 오브젝트

    [Header("QuantityLimit")]
    public bool canStack; //다중 보유가능 유무 true= 다중, false=한개만
    public int maxStackAmount; //최대 보유수량

    [Header("Consumable")]
    public ItemDataConsumabale[] consumabale; //아이템 효과값 배열로 만듬 
}
