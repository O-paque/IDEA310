using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public ResourceType resourceType;
    public int amount = 1;

    [Header("Sounds")]
    [SerializeField] private TriggeredSoundEffect pickupSound;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (InventoryManager.Instance != null && InventoryManager.Instance.AddResource(resourceType, amount))
        {
            pickupSound?.PlayAndDetach();
            Destroy(gameObject);
        }
    }
}
