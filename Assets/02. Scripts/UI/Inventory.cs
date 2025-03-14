using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
   public List<ItemData> items = new List<ItemData> (); // ����Ʈ�� ������ ����

    public void AddItem(ItemData newItem)
    {
        items.Add(newItem);
        Debug.Log(newItem /*.itemName*/ + "�� �߰��Ǿ����ϴ�!"); // �������� ������ �ִ� �̸��� �ִٸ� ���
    }

    public void RemoveItem(ItemData item)
    {
        items.Remove(item);
        Debug.Log(item /*.itemName*/ + "�� �����Ǿ����ϴ�!"); // �������� ������ �ִ� �̸��� �ִٸ� ���
    }
}
