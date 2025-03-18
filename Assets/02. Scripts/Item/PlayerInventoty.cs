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
    private Dictionary<Resource, int> ResourceAmount = new Dictionary<Resource, int>();


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


    //모든 상태를 false로 만드는 메서드
    private void ResetActive()
    {
        titleText.gameObject.SetActive(false);
        infoText.gameObject.SetActive(false);
        inventoryButton.ButtonSetActive(false, false, false, false);
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
                CountResource();
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
            CountResource();
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
            CountResource();
        }
    }


    //플레이어의 모든 인벤토리 칸을 순회하며 Resource 데이터의 총 합을 가져오는 메서드
    public void CountResource()
    {
        ResourceAmount = new Dictionary<Resource, int>();

        for(int i = 0; i < inventorySlots.Length; i++)
        {
            if(inventorySlots[i].itemObject != null &&
                inventorySlots[i].itemObject.data.resourceType != Resource.None)
            {
                if (ResourceAmount.ContainsKey(inventorySlots[i].itemObject.data.resourceType))
                {
                    ResourceAmount[inventorySlots[i].itemObject.data.resourceType] += inventorySlots[i].itemAmount;
                }
                else
                {
                    ResourceAmount.Add(inventorySlots[i].itemObject.data.resourceType, inventorySlots[i].itemAmount);
                }
            }
        }
    }

    public bool CreateItem(ItemData data)
    {
        //생성 가능한 상태인지 확인하기
        for(int i = 0; i < data.resources.Length; i++)
        {
            if (!(ResourceAmount.ContainsKey(data.resources[i].type)) ||
                ResourceAmount[data.resources[i].type] >= data.resources[i].Amount)
            {
                return false;
            }
        }

        //아이템 슬롯에서 해당하는 Resource를 확인하고 삭제하기
        for(int i = 0; i < data.resources.Length; i++)
        {
            int reduceCount = data.resources[i].Amount;
            while(reduceCount > 0)
            {
                int index = FindResourceIndex(data.resources[i].type);
                if (index == -1) 
                {
                    Debug.LogError("index를 찾지 못하는 오류 발생");
                    return false;
                }
                inventorySlots[index].ReduceItem();
                reduceCount--;
            }
        }

        //아이템 하나를 생산해서 Inventory에 추가하기
        GameObject item = new GameObject("item");
        item.AddComponent<ItemObject>().data = data;

        CheckItemSlot(item.GetComponent<ItemObject>());

        return true;
    }


    private int FindResourceIndex(Resource resource)
    {
        for(int i = 0; i < inventorySlots.Length; i++)
        {
            if (inventorySlots[i].itemObject.data.resourceType == resource)
            {
                return i;
            }
        }

        return -1;
    }
}
