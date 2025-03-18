using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ResourceSpawner : MonoBehaviour
{
    public float respawnTime = 5f;
    public int OriginCapacity = 5;
    
    public void SpawnStart()
    {
        StartCoroutine(RespawnResource(respawnTime));
    }

    private IEnumerator RespawnResource(float time)
    {
        yield return new WaitForSeconds(time / 3);
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(0).localScale = new Vector3(0.3f, 0.3f, 0.3f); 
        yield return new WaitForSeconds(time / 3);
        transform.GetChild(0).localScale = new Vector3(0.6f, 0.6f, 0.6f);
        yield return new WaitForSeconds(time / 3);
        transform.GetChild(0).localScale = new Vector3(1.0f, 1.0f, 1.0f);
        GetComponentInChildren<ResourceObject>().capacity = OriginCapacity;
    }
}
