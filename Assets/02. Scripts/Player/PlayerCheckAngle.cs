using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCheckAngle : MonoBehaviour
{
    CharacterController controller;
    public Vector3 repForce = Vector3.zero;
    private void OnValidate()
    {
        controller = transform.GetComponentDebug<CharacterController>();
    }


    //현재 부딪히고 있는 바닥의 각도가 45도 이상일 경우 플레이어를 밀어서 떨어뜨리는 메서드
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        float angle = Vector3.Angle(hit.normal, Vector3.up);
        if (angle > 45)
        {
            repForce = new Vector3
            (
                hit.normal.x * hit.normal.y * (angle / 10f - 4) * 2,
                0,
                hit.normal.z * hit.normal.y * (angle / 10f - 4) * 2
            );
        }
        else
        {
            repForce = Vector3.zero;
        }
    }
}
