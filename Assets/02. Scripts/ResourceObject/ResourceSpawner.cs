using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSpawner : MonoBehaviour
{
   public class ResourceSpawnPoint
    {
        public GameObject resourceObject; // 자원 오브젝트 지정
        public float respawnTime = 3; // 다시 돌아오는 시간
    }

    public List<ResourceSpawnPoint> resourceSpawnPoints = new List<ResourceSpawnPoint>(); // 리스트에 자원 오브젝트 저장 

    public void Start()
    {
       for(int i = 0; i < resourceSpawnPoints.Count; i++)
        {
            resourceSpawnPoints[i].resourceObject.SetActive(true); // 게임 시작할때, 모든 자원 오브젝트 활성화
        }
    }

    public void ResourceRegen(ResourceSpawnPoint resource) // 리스트 값에 변동 발생시, 코루틴 시작
    {
        if(resourceSpawnPoints.Count > 0)
        {
            StartCoroutine(RespawnResource(resource));
        }
       
    }

    private IEnumerator RespawnResource(ResourceSpawnPoint resource)
    {
        resource.resourceObject.SetActive(false);
        yield return new WaitForSeconds(resource.respawnTime);
        resource.resourceObject.SetActive(true);
    }
}
