using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public ResourceType resourceType;
    public int amount = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        InventoryManager inventory = other.GetComponent<InventoryManager>();

        if (inventory != null && inventory.AddResource(resourceType, amount))
        {
            Destroy(gameObject);
        }
    }
}
