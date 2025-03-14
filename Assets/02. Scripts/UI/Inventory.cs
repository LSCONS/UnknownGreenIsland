using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
   public List<ItemData> items = new List<ItemData> (); // 리스트에 아이템 저장

    public void AddItem(ItemData newItem)
    {
        items.Add(newItem);
        Debug.Log(newItem + "이 추가되었습니다!"); // 아이템이 가지고 있는 이름이 있다면 출력
    }

    public void RemoveItem(ItemData item)
    {
        items.Remove (item);
        Debug.Log(item +"이 삭제되었습니다!"); // 아이템이 가지고 있는 이름이 있다면 출력
    }
}
