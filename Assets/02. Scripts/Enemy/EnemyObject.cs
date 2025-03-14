using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyObject : MonoBehaviour
{
    public List<GameObject> itemToGive;
    public int Hp;

    public void HealthChange(Vector3 hitPoint, Vector3 hitNormal)
    {
        Hp -= 1;

        if (Hp <= 0)
        {

            death();
        }
    }

    private void death()
    {
        int RnadomNum = Random.RandomRange(0, itemToGive.Count);
        Util.ShuffleList<GameObject>(itemToGive);
        for (int j = 0; j < RnadomNum; j++)
        {
            Instantiate(itemToGive[j], transform.position + Vector3.up, Quaternion.LookRotation(Vector3.forward,Vector3.up));
        }

        Destroy(gameObject);
    }
}
