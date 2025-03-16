using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public void OnInteract();
}

public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemData data;


    public void OnInteract()
    {
        //TODO: 상호작용할 경우 처리를 결정하는 명령어 필요
        Destroy(gameObject);
    }
}

