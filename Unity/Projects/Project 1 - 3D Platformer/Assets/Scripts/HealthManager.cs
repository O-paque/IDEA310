using UnityEngine;
using System.Collections;

public class HealthManager : MonoBehaviour
{
    public CharacterController player;
    private bool isRespawning = false;
    private Vector3 respawnPoint;

    public int maxHealth = 2;
    public int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        //player = GetComponent<CharacterController>();
        respawnPoint = player.transform.position; 
    }

    void Update()
    {

    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        
        if (currentHealth <= 0)
        {
            Respawn(); 
        }
    }

    public void HealPlayer(int healAmount)
    {
        currentHealth += healAmount; 
        // Ensure current health does not exceed max health
        if (currentHealth > maxHealth) { currentHealth = maxHealth; }
    }

    public void Respawn()
    {
        if (!isRespawning)
        {
            StartCoroutine("RespawnCoroutine");
        }
    }

    public IEnumerator RespawnCoroutine()
    {
        isRespawning = true;
        player.enabled = false;
        player.gameObject.SetActive(false); 

        yield return new WaitForSeconds(3f);

        isRespawning = false;
        player.transform.position = respawnPoint;
        player.gameObject.SetActive(true);
        player.enabled = true;
        player.Move(Vector3.zero); 
        currentHealth = maxHealth; 
    }

}
