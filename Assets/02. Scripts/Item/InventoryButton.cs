using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryButton : MonoBehaviour
{
    private PlayerInventoty playerInventoty;
    private Transform inventoryCanvas;
    private Button equipButton;
    private Button unEquipButton;
    private Button useButton;
    private Button discardButton;

    private void OnValidate()
    {
        inventoryCanvas = transform.GetTransformInParent("UI_Inventory_Canvas");
        playerInventoty = inventoryCanvas.GetComponentForTransformFindName<PlayerInventoty>("PlayerInventory");
        equipButton = transform.GetComponentForTransformFindName<Button>("EquipButton");
        unEquipButton = transform.GetComponentForTransformFindName<Button>("UnEquipButton");
        useButton = transform.GetComponentForTransformFindName<Button>("UseButton");
        discardButton = transform.GetComponentForTransformFindName<Button>("DiscardButton");

        equipButton.onClick.AddListener(playerInventoty.EquippedItem);
        unEquipButton.onClick.AddListener(playerInventoty.UnEquippedItem);
        useButton.onClick.AddListener(playerInventoty.UseItem);
        discardButton.onClick.AddListener(playerInventoty.DiscardItem);
    }
}
