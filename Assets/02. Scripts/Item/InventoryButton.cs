using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryButton : MonoBehaviour
{
    private PlayerInventoty playerInventoty;
    private PlayerStatus playerStatus;
    private Transform inventoryCanvas;
    private Button equipButton;
    private Button unEquipButton;
    private Button useButton;
    private Button discardButton;

    private void OnValidate()
    {
        inventoryCanvas = transform.GetTransformInParent("UI_Inventory_Canvas");
        playerInventoty = inventoryCanvas.GetComponentForTransformFindName<PlayerInventoty>("PlayerInventory");
        playerStatus = transform.GetComponentInparentDebug<PlayerStatus>();
        equipButton = transform.GetComponentForTransformFindName<Button>("EquipButton");
        unEquipButton = transform.GetComponentForTransformFindName<Button>("UnEquipButton");
        useButton = transform.GetComponentForTransformFindName<Button>("UseButton");
        discardButton = transform.GetComponentForTransformFindName<Button>("DiscardButton");

        equipButton.onClick.AddListener(playerInventoty.EquippedItem);
        unEquipButton.onClick.AddListener(playerInventoty.UnEquippedItem);
        useButton.onClick.AddListener(playerInventoty.UseItem);
        discardButton.onClick.AddListener(playerInventoty.DiscardItem);
    }


    private void OnEnable()
    {
        CheckEquippedWeapon();
    }

    public void CheckEquippedWeapon()
    {
        ButtonSetActive(false, false, false);
        if (playerStatus.IsWeapon)
        {
            unEquipButton.gameObject.SetActive(true);
        }
        else
        {
            unEquipButton.gameObject.SetActive(false);
        }
    }


    public void ButtonCheckType(ItemType type)
    {
        switch(type)
        {
            case ItemType.Equipable:
                ButtonSetActive(true, false, true);
                break;
            case ItemType.Resource:
                ButtonSetActive(false, false, true);
                break;
            case ItemType.Consumable:
                ButtonSetActive(false, true, true);
                break;
        }
    }


    public void ButtonSetActive(bool equip, bool use, bool discard)
    {
        equipButton.gameObject.SetActive(equip);
        useButton.gameObject.SetActive(use);
        discardButton.gameObject.SetActive(discard);
    }
}
