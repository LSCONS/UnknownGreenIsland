using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using VInspector;

public class PlayerInventoty : MonoBehaviour
{
    [ShowInInspector, ReadOnly]
    private Transform inventoryCanvas;
    private InventorySlot[] inventorySlots;
    public int selectItemSlotIndex = -1;
    private TextMeshProUGUI titleText;
    private TextMeshProUGUI infoText;
    private InventoryButton inventoryButton;


    private void OnValidate()
    {
        inventoryCanvas = transform.GetTransformInParent("UI_Inventory_Canvas");
        inventoryButton = inventoryCanvas.GetComponentForTransformFindName<InventoryButton>("Button");
        inventorySlots = transform.GetComponentsInChildren<InventorySlot>();
        for(int i = 0; i < inventorySlots.Length; i++)
        {
            inventorySlots[i].slotIndex = i;
        }
        titleText = transform.parent.parent.GetComponentForTransformFindName<TextMeshProUGUI>("ItemName");
        infoText = transform.parent.parent.GetComponentForTransformFindName<TextMeshProUGUI>("ItemDescription");
    }


    private void OnEnable()
    {
        ResetActive();
    }



    /// <summary>
    /// 넣을 수 있는 아이템 칸이 있는지 확인하고 아이템을 집어넣는 메서드
    /// </summary>
    /// <param name="itemObject">집어넣을 아이템 오브젝트</param>
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


    /// <summary>
    /// 아이템이 선택된 경우 상호작용이 가능한 버튼을 활성화하고 해당 아이템 슬롯의 번호를 지정하는 메서드
    /// </summary>
    /// <param name="index">해당 명령을 실행할 아이템슬롯의 index번호</param>
    public void SelectedItemSlot(int index)
    {
        selectItemSlotIndex = index;
        titleText.text = inventorySlots[selectItemSlotIndex].itemObject.data.ItemName;
        infoText.text = inventorySlots[selectItemSlotIndex].itemObject.data.description;
        titleText.gameObject.SetActive(true);
        infoText.gameObject.SetActive(true);

        inventoryButton.ButtonCheckType(inventorySlots[selectItemSlotIndex].itemObject.data.type);
    }


    /// <summary>
    /// 현재 선택된 아이템 칸의 아이템을 사용하는 메서드
    /// </summary>
    public void UseItem()
    {
        if (selectItemSlotIndex != -1)
        {
            inventorySlots[selectItemSlotIndex].UseItem();
            if (inventorySlots[selectItemSlotIndex].itemObject == null)
            {
                ResetActive();
            }
        }
    }

    
    /// <summary>
    /// 선택한 아이템을 장비하는 메서드
    /// </summary>
    public void EquippedItem()
    {
        //TODO: 선택한 아이템을 장비 필요
    }


    /// <summary>
    /// 선택한 장비를 해제하는 메서드
    /// </summary>
    public void UnEquippedItem()
    {
        //TODO: 장비한 아이템 해제 필요
    }


    /// <summary>
    /// 선택한 아이템을 버리는 메서드
    /// </summary>
    public void DiscardItem()
    {
        if (selectItemSlotIndex != -1)
        {
            inventorySlots[selectItemSlotIndex].RemoveItem();
            if (inventorySlots[selectItemSlotIndex].itemObject == null)
            {
                ResetActive();
            }
        }
    }

    private void ResetActive()
    {
        titleText.gameObject.SetActive(false);
        infoText.gameObject.SetActive(false);
        inventoryButton.ButtonSetActive(false, false, false, false);
    }
}
