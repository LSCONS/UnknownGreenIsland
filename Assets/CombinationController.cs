using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombinationController : MonoBehaviour
{
    public GameObject combinationSlot;

    public void CreateCombinationSlot(Dictionary<int, ItemObject> dict)
    {
        foreach(var key in dict.Keys)
        {
            GameObject temp = Instantiate(combinationSlot, transform);
            CombinationSlot tempComb = temp.GetComponent<CombinationSlot>();
            if (tempComb != null)
            {
                tempComb.UpdateData(dict[key]);
            }
        }
    }
}
