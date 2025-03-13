using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    Resources,
    Combat
}

public class WeaponHandler : MonoBehaviour
{
    Animator animator;
    Camera camera;

    public WeaponType weaponType;
    public int attackDistance; //공격 사거리

    private void Awake()
    {
        animator = GetComponent<Animator>();
        camera = Camera.main;
    }

    public void Attack()
    {
        animator.SetBool("IsAttack", true);

        Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2 + 1.5f , 0 ));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, attackDistance, ReadonlyData.ResourceObjectLayerMask | ReadonlyData.EnemyLayerMask))
        {
            if (hit.collider.TryGetComponent(out ResourceObject resource) && WeaponType.Resources == weaponType)
            {
                resource.Gather(hit.point, hit.normal);
            }
            else if(hit.collider.TryGetComponent(out EnemyObject Enemy) && WeaponType.Combat == weaponType)
            {
                Enemy.HealthChange(hit.point, hit.normal);
            }
        }
    }

    public void StopAttack()
    {
        animator.SetBool("IsAttack", false);
    }
}
