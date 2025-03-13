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
        // �浹�� ������Ʈ ���� Ȯ��
        //Debug.Log("�浹�� ������Ʈ: " + hit.gameObject.name);

        // �浹�� ���� ���� ���͸� �̿��� �ٴ� ���� ���� �߰� ���� ���� ����
        float angle = Vector3.Angle(hit.normal, Vector3.up);
        if (angle > 45)
        {
            repForce = new Vector3(hit.normal.x, 0, hit.normal.z).normalized;
            // ����: ���̳� ��簡 �ʹ� ū ���̸� �߰� �ϰ� ó��
            Debug.Log("���̳� ���ο� �浹: �߰� �ϰ� ó�� �ʿ�");
        }
        else
        {
            repForce = Vector3.zero;
        }
    }
}
