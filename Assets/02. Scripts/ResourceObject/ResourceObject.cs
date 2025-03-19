using UnityEngine;

public enum ResourceObjectType
{
    None,
    Tree,
    Rock
}

public class ResourceObject : MonoBehaviour
{
    public ItemData itemToGive;
    public int quantityPerHit = 1;
    public int capacity;
    public ResourceObjectType resourceObjectType;

    private void Start()
    {
        capacity = GetComponentInParent<ResourceSpawner>().OriginCapacity;
    }

    public void Gather(Vector3 hitPoint, Vector3 hitNormal, ResourceObjectType type)
    {
        if (resourceObjectType == type)
        {
            for (int i = 0; i < quantityPerHit; i++)
            {
                if (capacity <= 0) break;

                capacity -= 1;

                Instantiate(itemToGive.dropPrefab, hitPoint + Vector3.up, Quaternion.LookRotation(hitNormal, Vector3.up));
            }


            if (capacity <= 0)
            {
                gameObject.SetActive(false);
            }
        }
    }

    public void OnDisable()
    {
        GetComponentInParent<ResourceSpawner>()?.SpawnStart();
    }
}
