using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using VInspector;

public class InventorySlot : MonoBehaviour
{
    public int slotIndex;
    [ShowInInspector, ReadOnly]
    private Image icon;
    private TextMeshProUGUI text;
    public ItemObject itemObject;
    private int itemAmount = 0;
    private GameObject _objectPool;
    private Button button;
    [ShowInInspector, ReadOnly]
    private PlayerInventoty inventorySlotGrid;

    private void OnValidate()
    {
        text = transform.GetComponentForTransformFindName<TextMeshProUGUI>("StackCount");
        icon = transform.GetComponentForTransformFindName<Image>("Icon");
        _objectPool = transform.GetComponentForTransformFindName<Transform>("ObjectPool").gameObject;
        button = transform.GetComponentDebug<Button>();
        inventorySlotGrid = GetComponentInParent<PlayerInventoty>();
    }


    private void Awake()
    {
        UpdateAmountText();
        UpdateIcon();
    }


    //해당 아이템을 해당 칸에 집어넣는 메서드
    private void OnInputItem(ItemObject inputItemObject)
    {
        if (itemObject == null)
        {
            itemObject = inputItemObject;
            icon.sprite = itemObject.data.inventory_icon;
            button.onClick.AddListener(SelectedSlot);
            UpdateIcon();
        }

        if (itemObject.data.canStack)
        {
            itemAmount++;
            UpdateAmountText();
        }

        inputItemObject.transform.SetParent(_objectPool.transform);
    }


    //아이템의 텍스트를 초기화하는 메서드
    private void UpdateAmountText()
    {
        if (itemAmount == 0)
        {
            text.gameObject.SetActive(false);
        }
        else
        {
            text.text = itemAmount.ToString();
            text.gameObject.SetActive(true);
        }
    }


    //아이템의 아이콘을 초기화하는 메서드
    private void UpdateIcon()
    {
        if (itemObject == null)
        {
            icon.gameObject.SetActive(false);
        }
        else
        {
            icon.gameObject.SetActive(true);
        }
    }


    //아이템 슬롯을 선택할 때 실행할 메서드
    private void SelectedSlot()
    {
        inventorySlotGrid.SelectedItemSlot(slotIndex);
    }


    //아이템을 제거할 때 실행할 메서드
    private void ReduceItem()
    {
        if (itemObject.data.canStack)
        {
            itemAmount--;
            UpdateAmountText();
        }

        if (itemAmount == 0)
        {
            itemObject = null;
            UpdateIcon();
            button.onClick.RemoveListener(SelectedSlot);
        }
    }


    /// <summary>
    /// 현재 아이템 칸에 들어갈 수 있는 아이템 칸이 있는지 확인하고 집어넣는 메서드
    /// </summary>
    /// <param name="inputItemObject">집어넣을 아이템의 ItemObject</param>
    /// <returns>아이템을 넣었는지 못 넣었는지 bool값 반환</returns>
    public bool CheckInputItem(ItemObject inputItemObject)
    {
        if (itemObject == null ||
            (itemObject.data.canStack &&
            itemObject.data.ItemID == inputItemObject.data.ItemID &&
            itemAmount < itemObject.data.maxStackAmount))
        {
            OnInputItem(inputItemObject);
            return true;
        }
        return false;
    }


    /// <summary>
    /// 사용 아이템을 사용하는 메서드.
    /// </summary>
    public void UseItem()
    {
        //TODO: 사용 아이템 사용하는 명령어 필요
        ReduceItem();
    }
}
