using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VInspector;

public class PlayerIsgrounded : MonoBehaviour
{
    
    public LayerMask includeLayerMask;

     public bool isgrounded;
    public bool Isgrounded { get => isgrounded; }

    private void OnValidate()
    {
        //적 레이어와 플레이어 레이어가 아닌 경우
        includeLayerMask = (ReadonlyData.GroundLayerMask | ReadonlyData.InteractionLayerMask | ReadonlyData.ResourceObjectLayerMask);
    }


    //지속적으로 닿고 있는 레이어가 _excludeLayerMask와 같을 경우 땅에 닿고 있음을 표시
    private void OnTriggerStay(Collider other)
    {
        if(includeLayerMask == (includeLayerMask | 1 << other.gameObject.layer))
        {
            Debug.Log("stay gameObject = " +  other.gameObject);
            isgrounded = true;
        }
    }


    //_excludeLayerMask 레이어에서 벗어난 경우 땅에서 벗어남을 표시
    private void OnTriggerExit(Collider other)
    {
        if (includeLayerMask == (includeLayerMask | 1 << other.gameObject.layer))
        {
            Debug.Log("exit gameObject = " + other.gameObject);
            isgrounded = false;
        }
    }
}
