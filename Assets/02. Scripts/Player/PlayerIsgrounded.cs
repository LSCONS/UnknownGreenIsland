using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VInspector;

public class PlayerIsgrounded : MonoBehaviour
{
    
    public LayerMask _excludeLayerMaslk;

    [ShowInInspector]
    private bool isgrounded;
    public bool Isgrounded { get => isgrounded; }

    private void OnValidate()
    {
        _excludeLayerMaslk = ~(ReadonlyData.EnemyLayerMask | ReadonlyData.PlayerLayerMask);
    }


    private void OnTriggerStay(Collider other)
    {
        if(_excludeLayerMaslk == (_excludeLayerMaslk | 1 << other.gameObject.layer))
        {
            isgrounded = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_excludeLayerMaslk == (_excludeLayerMaslk | 1 << other.gameObject.layer))
        {
            isgrounded = false;
        }
    }
}
