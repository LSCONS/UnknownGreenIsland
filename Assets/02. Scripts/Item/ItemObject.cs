using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    public ItemData data;

    private void OnValidate()
    {
        data = Resources.Load<ItemData>($"ItemData/{gameObject.name}"); 
    }
}

