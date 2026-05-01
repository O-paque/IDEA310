using UnityEngine;

[System.Serializable]
public class ResourceDropItem
{
    public ResourceType resourceType;
    public int minAmount = 1;
    public int maxAmount = 3;
    public GameObject pickupPrefab;
    [Range(0f, 1f)] public float dropChance = 1f;
}

public class ResourceDrop : MonoBehaviour
{
    public ResourceDropItem[] drops;

    public void Drop()
    {
        foreach (var drop in drops)
        {
            if (Random.value > drop.dropChance) continue;

            int amount = Random.Range(drop.minAmount, drop.maxAmount + 1);

            for (int i = 0; i < amount; i++)
            {
                Vector3 spawnPos = transform.position + Random.insideUnitSphere * 1.5f;
                spawnPos.y = transform.position.y + 0.5f;

                GameObject obj = Instantiate(drop.pickupPrefab, spawnPos, Quaternion.identity);

                ItemPickup pickup = obj.GetComponent<ItemPickup>();
                if (pickup != null)
                {
                    pickup.resourceType = drop.resourceType;
                    pickup.amount = 1;
                }
            }
        }
    }
}