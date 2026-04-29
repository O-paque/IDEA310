using UnityEngine;

public class DealDamage : MonoBehaviour
{
    public int damageAmount = 10;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (HealthManager.Instance != null)
            {
                HealthManager.Instance.TakeDamage(damageAmount);
                Debug.Log($"Player hit for {damageAmount} damage.");
            }
        }
    }
}
