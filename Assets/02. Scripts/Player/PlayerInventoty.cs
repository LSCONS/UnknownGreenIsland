using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoty : MonoBehaviour
{
    //TODO: 역할
    //플레이어 인벤토리 토글 기능
    //플레이어 인벤토리와 연결
    private PlayerInput input;


    private void OnValidate()
    {
        input = transform.GetComponentInparentDebug<PlayerInput>(); 
    }


    private void Start()
    {
        input.inventoryAction += InventoryToggle;
    }


    private void InventoryToggle()
    {

    }
}
