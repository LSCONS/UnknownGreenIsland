using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    private Camera _camera;
    private GameObject currentObject;
    private float distanceMax = 5f;
    private LayerMask interactionLayerMask;
    private ItemObject itemObject;
    private PlayerInput playerInput;
    private UIInteraction uiInteraction;
    private PlayerInventoty playerInventoty;
    private float tempime = 0;

    public Action craftingToggle;


    private void OnValidate()
    {
        playerInventoty = transform.GetComponentForTransformFindName<PlayerInventoty>("PlayerInventory");
        playerInput = transform.GetComponentDebug<PlayerInput>();
        uiInteraction  = transform.GetComponentForTransformFindName<UIInteraction>("UIInteraction");
        interactionLayerMask = ReadonlyData.InteractionLayerMask | ReadonlyData.InteractionCookLayerMask | ReadonlyData.InteractionWorkLayerMask;
        _camera = Camera.main;
    }

    private void Start()
    {
        uiInteraction.UpdateData(itemObject);
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
                uiInteraction.UpdateData(itemObject);
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
                uiInteraction.UpdateData(itemObject);
            }
        }
    }


    //레이어에 따라 상호작용을 나눠서 적용시킴.
    private void InteractionHandler()
    {
        if (currentObject != null)
        {
            Debug.Log(currentObject.layer);
            LayerMask nowLayer = 1 << currentObject.layer;

            if(nowLayer == ReadonlyData.InteractionLayerMask)
            {
                InteractionItem(itemObject);
            } 
            else if (nowLayer == ReadonlyData.InteractionCookLayerMask)
            {
                InteractionCookingTable();
            }
            else if (nowLayer == ReadonlyData.InteractionWorkLayerMask)
            {
                InteractionCraftingTable();
            }
        }
    }


    //인벤토리에 남은 칸을 확인하고 아이템을 집어넣는 메서드
    public void InteractionItem(ItemObject itemObject)
    {
        playerInventoty.CheckItemSlot(itemObject);
    }


    //문과 상호작용을 할 경우 실행할 메서드
    public void InteractionFabic()
    {

    }


    //조합대와 상호작용을 할 경우 실행할 메서드
    public void InteractionCraftingTable()
    {
        //조합대 관련된 UI와 연결
        //인벤토리 토글 상태 변경
        playerInput.IsInventoryToggle();

        //TODO: 해당 오브젝트와 관련된 조합 식 업로드

        craftingToggle?.Invoke();
    }


    //조리대와 상호작용을 할 경우 실행할 메서드
    public void InteractionCookingTable()
    {
        //인벤토리 토글 상태 변경
        playerInput.IsInventoryToggle();

        //TODO: 해당 오브젝트와 관련된 조합 식 업로드

        craftingToggle?.Invoke();
    }
}
