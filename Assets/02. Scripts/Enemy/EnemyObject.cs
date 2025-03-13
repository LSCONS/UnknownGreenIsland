using System.Collections;
using System.Collections.Generic;
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
            int RnadomNum = Random.RandomRange(0, itemToGive.Count);
            Util.ShuffleList<GameObject>(itemToGive);
            for (int j = 0; j < RnadomNum; j++)
            {
                Instantiate(itemToGive[j], hitPoint + Vector3.up, Quaternion.LookRotation(hitNormal, Vector3.up));
            }
            death();
        }
    }

    private void death()
    {
        Destroy(gameObject);
    }
}
