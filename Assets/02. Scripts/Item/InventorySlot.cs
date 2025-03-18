using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using VInspector;

public class InventorySlot : MonoBehaviour
{
    private Image icon;
    private TextMeshProUGUI text;
    private GameObject objectPool;
    private Button button;
    private PlayerInventoty playerInventory;
    private PlayerInput playerinput;
    private PlayerStatus playerStatus;
    public int itemAmount = 0;


    public ItemObject itemObject;
    public int slotIndex;

    private void OnValidate()
    {
        text = transform.GetComponentForTransformFindName<TextMeshProUGUI>("QuantityText");
        icon = transform.GetComponentForTransformFindName<Image>("Icon");
        objectPool = transform.GetComponentForTransformFindName<Transform>("ObjectPool").gameObject;
        button = transform.GetComponentDebug<Button>();
    }


    private void Awake()
    {
        playerinput = transform.GetComponentInparentDebug<PlayerInput>();
        playerInventory = transform.GetComponentInparentDebug<PlayerInventoty>();
        playerStatus = transform.GetComponentInparentDebug<PlayerStatus>();
        UpdateAmountText();
        UpdateIcon();
    }


    //아이템을 장비 아이템으로 옮기고 해당 아이템 칸을 비움.
    public void EquippedItem(Transform weaponPivot)
    {
        itemObject.transform.parent = weaponPivot;
        Animator animator = itemObject.GetComponent<Animator>();
        playerinput.Attack += animator.SetBool;
        itemObject.transform.localPosition = new Vector3(0, 0.1f, -0.2f);
        itemObject.transform.localRotation = Quaternion.Euler(-15, -100, -10);
        itemObject.gameObject.SetActive(true);
        RemoveItem();
    }


    //해당 아이템을 해당 칸에 집어넣는 메서드
    private void OnInputItem(ItemObject inputItemObject)
    {
        if (itemObject == null)
        {
            itemObject = inputItemObject;
            icon.sprite = itemObject.data.inventory_icon;
            button.onClick.AddListener(SelectedSlot);
            itemObject.gameObject.SetActive(false);
            UpdateIcon();
        }

        if (itemObject.data.canStack)
        {
            itemAmount++;
            UpdateAmountText();
        }

        inputItemObject.transform.SetParent(objectPool.transform);
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
        playerInventory.SelectedItemSlot(slotIndex);
    }


    //아이템을 제거할 때 실행할 메서드
    public void ReduceItem()
    {
        if (itemObject.data.canStack)
        {
            itemAmount--;
            UpdateAmountText();
        }

        if (itemAmount == 0)
        {
            RemoveItem();
        }
    }


    //입력받은 타입과 value를 Status에 적용시키는 메서드
    private void ChangeValue(ConsumableType type, float value)
    {
        switch (type)
        {
            case ConsumableType.Health:
                playerStatus.HealthChange(value);
                break;
            case ConsumableType.Stamina:
                playerStatus.StaminaChange(value);
                break;
            case ConsumableType.Hunger:
                playerStatus.HungerChange(value);
                break;
            case ConsumableType.Thirsty:
                playerStatus.ThirstyChange(value);
                break;
        }
    }


    /// <summary>
    /// 아이템을 삭제할 때 사용하는 메서드
    /// </summary>
    public void RemoveItem()
    {
        itemAmount = 0;
        itemObject = null;
        UpdateIcon();
        UpdateAmountText();
        button.onClick.RemoveListener(SelectedSlot);
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
        if (itemObject.data.consumabale.Length > 0)
        {
            for (int i = 0; i < itemObject.data.consumabale.Length; i++)
            {
                ChangeValue(itemObject.data.consumabale[i].type, itemObject.data.consumabale[i].value);
            }
        }
        ReduceItem();
    }
}
