using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public ResourceType resourceType;
    public int amount = 1;
    private bool collected = false;

    [Header("Sounds")]
    [SerializeField] private TriggeredSoundEffect pickupSound;

    private void OnTriggerEnter(Collider other)
    {
        if (collected) return;
        if (!other.CompareTag("Player")) return;

        if (InventoryManager.Instance == null) return;

        bool added = InventoryManager.Instance.AddResource(resourceType, amount);

        if (added)
        {
            collected = true;

            Collider pickupCollider = GetComponent<Collider>();
            if (pickupCollider != null)
                pickupCollider.enabled = false;
            pickupSound?.PlayAndDetach();
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
