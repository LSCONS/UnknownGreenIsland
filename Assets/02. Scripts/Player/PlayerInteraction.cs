using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    public Action interaction;
    private Camera _camera;
    private GameObject currentObject;
    private float distanceMax = 5f;
    private LayerMask interactionLayerMask;
    private ItemObject itemObject;
    private PlayerInput playerInput;
    private float tempime;


    private void OnValidate()
    {
        playerInput = transform.GetComponentDebug<PlayerInput>();
        interactionLayerMask = ReadonlyData.InteractionLayerMask;
        _camera = Camera.main;
    }

    private void Update()
    {
        tempime += Time.deltaTime;
        if (tempime >= 0.1 && !(playerInput.IsInventory))
        {
            ShootingLayCastForCamera();
            tempime = 0;
        }
    }


    public void InteractionStart()
    {
        interaction?.Invoke();
    }


    //카메라 가운데로 레이케스트를 쏴서 상호작용 가능한 오브젝트를 확인하는 메서드  
    private void ShootingLayCastForCamera()
    {
        Ray _ray = _camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit _hit;

        if (Physics.Raycast(_ray, out _hit, distanceMax, interactionLayerMask))
        {
            if (_hit.collider.gameObject != currentObject)
            {
                currentObject = _hit.collider.gameObject;
                itemObject = currentObject.GetComponentInParent<ItemObject>();
                if (itemObject == null) Debug.LogError("itemObject is null");
                playerInput.interactionAction -= InteractionHandler;
                playerInput.interactionAction += InteractionHandler;
                //TODO: UI업데이트 필요
            }
        }
        else
        {
            if (itemObject != null)
            {
                playerInput.interactionAction -= InteractionHandler;
                itemObject = null;
            }

            if (currentObject != null)
            {
                currentObject = null;
                //TODO: UI업데이트 필요
            }
        }
    }


    //레이어에 따라 상호작용을 나눠서 적용시킴.
    private void InteractionHandler()
    {
        //TODO: 레이어에 따라 상호작용을 나눔.
        CheckItemSlot(itemObject);
    }


    //인벤토리에 남은 칸을 확인하고 아이템을 집어넣는 메서드
    private void CheckItemSlot(ItemObject item)
    {

    }
}
