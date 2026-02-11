using UnityEngine;

public class TakeDamage : MonoBehaviour
{
    public int damageAmount = 1; // Amount of damage to inflict on the player
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            FindFirstObjectByType<HealthManager>().TakeDamage(damageAmount); // Call TakeDamage on the HealthManager script attached to the player
        }
    }
}
