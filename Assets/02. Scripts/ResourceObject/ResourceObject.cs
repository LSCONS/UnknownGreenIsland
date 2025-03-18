using UnityEngine;

public class ResourceObject : MonoBehaviour
{
    public GameObject itemToGive;
    public int quantityPerHit = 1;
    public int capacity;

    private void Start()
    {
        capacity = GetComponentInParent<ResourceSpawner>().OriginCapacity;
    }

    public void Gather(Vector3 hitPoint, Vector3 hitNormal)
    {
        for (int i = 0; i < quantityPerHit; i++)
        {
            if (capacity <= 0) break;

            capacity -= 1;
            Instantiate(itemToGive, hitPoint + Vector3.up, Quaternion.LookRotation(hitNormal, Vector3.up));
        }

        if (capacity <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    public void OnDisable()
    {
        GetComponentInParent<ResourceSpawner>().SpawnStart();
    }
}
