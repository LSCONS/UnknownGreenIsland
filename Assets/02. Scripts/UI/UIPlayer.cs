using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPlayer : MonoBehaviour
{
    private Transform combinationBG;
    private Transform inventoryBG;
    private Transform infoBG;
    private Transform statBG;
    private PlayerInput input;
    private PlayerInteraction playerInteraction;

    private void OnValidate() 
    {
        combinationBG = transform.GetGameObjectSameNameDFS("CombinationBG");
        inventoryBG = transform.GetGameObjectSameNameDFS("InventoryBG");
        infoBG = transform.GetGameObjectSameNameDFS("InfoBG");
        statBG = transform.GetGameObjectSameNameDFS("StatBG");
        input = transform.GetComponentInparentDebug<PlayerInput>();
        playerInteraction = transform.GetComponentInparentDebug<PlayerInteraction>();
    }


    private void Start()
    {
        input.inventoryAction += InventorySetAcitve;
        input.inventoryExitAction += InventoryExit;
        playerInteraction.craftingToggle += CraftingSetActive;
        InventoryExit();
    }


    private void CraftingSetActive()
    {
        if (input.IsInventory)
        {
            inventoryBG.GetComponent<RectTransform>().anchoredPosition = Vector3.left * 250;
            combinationBG.gameObject.SetActive(true);
            inventoryBG.gameObject.SetActive(true);
            Util.CursorisLock(false);
        }
    }


    //인벤토리를 끄고 키면서 마우스 커서의 Lock을 바꾸는 메서드
    private void InventorySetAcitve()
    {
        inventoryBG.gameObject.SetActive(true);
        inventoryBG.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
        infoBG.gameObject.SetActive(true);
        statBG.gameObject.SetActive(true);
        Util.CursorisLock(false);
    }


    //인벤토리 창을 나갈 때 무조건 실행되게 하는 메서드
    private void InventoryExit()
    {
        combinationBG.gameObject.SetActive(false);
        inventoryBG.gameObject.SetActive(false);
        infoBG.gameObject.SetActive(false);
        statBG.gameObject.SetActive(false);
    }
}
