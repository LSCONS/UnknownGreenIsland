using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Player : MonoBehaviour
{
    private Transform conditionCanvas;
    private Transform inventoryCanvas;
    private Transform interactionCanvas;
    private PlayerInput input;

    private void OnValidate()
    {
        conditionCanvas = transform.GetGameObjectSameNameDFS("UI_Condition_Canvas");
        inventoryCanvas = transform.GetGameObjectSameNameDFS("UI_Inventory_Canvas");
        interactionCanvas = transform.GetGameObjectSameNameDFS("UI_Interaction_Canvas");
        input = transform.GetComponentInparentDebug<PlayerInput>();
    }


    private void Start()
    {
        input.inventoryAction += InventorySetAcitve;
        inventoryCanvas.gameObject.SetActive(false);
    }


    private void InventorySetAcitve()
    {
        inventoryCanvas.gameObject.SetActive(input.IsInventory);
        Util.CursorisLock(!(input.IsInventory));
    }
}
