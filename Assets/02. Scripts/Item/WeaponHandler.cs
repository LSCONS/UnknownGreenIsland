using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Weapontype
{
    Resources,
    Attack
}

public class WeaponHandler : MonoBehaviour
{
    CameraMoving cameraMoving;
    Animator animator;

    private void Awake()
    {
        cameraMoving = "Main Camera".GetComponentNameDFS<CameraMoving>();
        animator = GetComponentInChildren<Animator>();
    }
    private void LateUpdate()
    {
        transform.position = new Vector3(cameraMoving.transform.position.x + 0.75f, cameraMoving.transform.position.y - 0.5f, cameraMoving.transform.position.z + 1.2f);
    }

    public void Attack()
    {
        animator.SetBool("IsAttack", true);
    }
    public void StopAttack()
    {
        animator.SetBool("IsAttack", false);
    }
}
