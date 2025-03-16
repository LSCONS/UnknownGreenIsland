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


    //인벤토리를 끄고 키면서 마우스 커서의 Lock을 바꾸는 메서드
    private void InventorySetAcitve()
    {
        inventoryCanvas.gameObject.SetActive(input.IsInventory);
        Util.CursorisLock(!(input.IsInventory));
    }
}
