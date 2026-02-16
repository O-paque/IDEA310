using UnityEngine;

public class RespawnUpdate : MonoBehaviour
{

    private Vector3 respawnPoint;
    private HealthManager healthManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        healthManager = FindFirstObjectByType<HealthManager>();
        respawnPoint = transform.position;
        respawnPoint.y += 8f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            healthManager.setRespawnPoint(respawnPoint);
        }
    }
}
