using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum ItemType
{
    Equipable, //���� ������
    Consumable, //�Һ� ������
    Resource, //�ڿ� ������
}

public enum ConsumableType
{
    Health, //ü��
    Hunger, //�����
    Stamina, //���¹̳�

}

[Serializable]

public class ItemDataConsumabale
{
    public ConsumableType type; //Ÿ�� ����
    public float value; //������ ��� �� ���� ��
}

[CreateAssetMenu(fileName = "Item", menuName = "New Item")]
public class ItemData : ScriptableObject
{
    [Header("Info")]
    public string ItemName; //������ �̸�
    public string description; //������ ����
    public ItemType type; //������ Ÿ��
    public Sprite icon; //������ ������
    public GameObject dropPrefab; //������� �� ��Ÿ���� 3D ������Ʈ

    [Header("QuantityLimit")]
    public bool canStack; //���� �������� ���� true= ����, false=�Ѱ���
    public int maxStackAmount; //�ִ� ��������

    [Header("Consumable")]
    public ItemDataConsumabale[] consumabale; //������ ȿ���� �迭�� ����
}
