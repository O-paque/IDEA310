using UnityEngine;
using StarterAssets;

public class RespawnUpdate : MonoBehaviour
{
    private Vector3 respawnPoint;
    private HealthManager healthManager;

    [Header("Checkpoint Slide Settings")]
    [Range(0f, 2f)]
    public float slideStrengthAtCheckpoint = 1f;

    void Start()
    {
        healthManager = FindFirstObjectByType<HealthManager>();

        respawnPoint = transform.position;
        respawnPoint.y += 8f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            healthManager.setRespawnPoint(respawnPoint);

            
            ThirdPersonController controller =
                other.GetComponentInParent<ThirdPersonController>();

            if (controller != null)
            {
                controller.SetSlideEffectiveness(slideStrengthAtCheckpoint);
            }
        }
    }
}
