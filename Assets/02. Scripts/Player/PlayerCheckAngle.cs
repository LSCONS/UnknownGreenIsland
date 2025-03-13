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


    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // 충돌한 오브젝트 정보 확인
        //Debug.Log("충돌한 오브젝트: " + hit.gameObject.name);

        // 충돌한 면의 법선 벡터를 이용해 바닥 판정 등의 추가 로직 구현 가능
        float angle = Vector3.Angle(hit.normal, Vector3.up);
        if (angle > 45)
        {
            repForce = new Vector3(hit.normal.x, 0, hit.normal.z).normalized;
            // 예시: 벽이나 경사가 너무 큰 곳이면 추가 하강 처리
            Debug.Log("벽이나 경사로와 충돌: 추가 하강 처리 필요");
        }
        else
        {
            repForce = Vector3.zero;
        }
    }
}
