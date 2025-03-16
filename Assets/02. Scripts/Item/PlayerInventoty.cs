using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using VInspector;

public class PlayerInventoty : MonoBehaviour
{
    //TODO: 역할
    //플레이어 인벤토리 토글 기능
    //플레이어 인벤토리와 연결
    private PlayerInput input;
    [ShowInInspector, ReadOnly]
    private InventorySlot[] inventorySlots;
    public int selectItemSlotIndex = -1;
    private TextMeshProUGUI titleText;
    private TextMeshProUGUI infoText;


    //넣을 수 있는 아이템 칸이 있는지 확인하고 아이템을 집어넣는 메서드
    public void CheckItemSlot(ItemObject itemObject)
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (inventorySlots[i].CheckInputItem(itemObject))
            {
                itemObject.gameObject.SetActive(false);
                return;
            }
        }

        //TODO: 아이템 칸이 꽉 차서 아이템 칸을 찾을 수 없는 경우의 예외처리 명령어 필요 
    }


    //아이템이 선택된 경우 상호작용이 가능한 버튼을 활성화하고 해당 아이템 슬롯의 번호를 지정하는 메서드
    public void SelectedItemSlot(int index)
    {
        selectItemSlotIndex = index;
        titleText.text = inventorySlots[selectItemSlotIndex].itemObject.data.ItemName;
        infoText.text = inventorySlots[selectItemSlotIndex].itemObject.data.description;
        titleText.gameObject.SetActive(true);
        infoText.gameObject.SetActive(true);
    }


    //현재 선택된 아이템 칸의 아이템을 사용하는 메서드
    public void UseItem()
    {
        if (selectItemSlotIndex != -1)
        {
            inventorySlots[selectItemSlotIndex].UseItem();
            if (inventorySlots[selectItemSlotIndex].itemObject == null)
            {
                titleText.text = "";
                infoText.text = "";
            }
        }
    }


    //TODO: 아이템 던지는 메서드 추가 예정
    public void ThrowItem(int index)
    {

    }
}
