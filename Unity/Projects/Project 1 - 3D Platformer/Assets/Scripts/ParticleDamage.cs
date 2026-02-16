using UnityEngine;

public class ParticleDamage : MonoBehaviour
{
    public int damageAmount = 1; // Amount of damage to inflict on the player
    public float hitCooldown = 0.2f;
    private float lastHitTime = 0f;
    private void OnParticleCollision(GameObject other)
    {
        if (Time.time < lastHitTime) { return; } // Prevent multiple hits in quick succession)

        if (other.gameObject.tag == "Player")
        {
            FindFirstObjectByType<HealthManager>().TakeDamage(damageAmount);
            lastHitTime = Time.time + hitCooldown;
        }
    }
}
