using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int maxHealth = 50;
    private int currentHealth;

    [Header("Flash Settings")]
    [SerializeField] private float flashDuration = 0.1f;
    [SerializeField] private Color flashColor = Color.red;
    [Header("Random")]
    [SerializeField] private ObjectMoveOnDeath moveOnDeath1;
    [SerializeField] private ObjectMoveOnDeath moveOnDeath2;

    [Header("Sounds")]
    [SerializeField] private TriggeredSoundEffect hitSound;
    [SerializeField] private TriggeredSoundEffect deathSound;

    private Renderer[] renderers;
    private Color[] originalColors;

    private void Awake()
    {
        currentHealth = maxHealth;

        renderers = GetComponentsInChildren<Renderer>();
        originalColors = new Color[renderers.Length];

        for (int i = 0; i < renderers.Length; i++)
        {
            originalColors[i] = renderers[i].material.color;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0)
            currentHealth = 0;

        Debug.Log($"{gameObject.name} took {damage} damage. Current health: {currentHealth}");
        
        hitSound?.PlaySound();
        StartCoroutine(FlashRed());

        if (currentHealth <= 0)
        {
            hitSound?.PlayAndDetach();
            Die();
        }
    }

    private IEnumerator FlashRed()
    {
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material.color = flashColor;
        }

        yield return new WaitForSeconds(flashDuration);

        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material.color = originalColors[i];
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} died.");
        moveOnDeath1?.TriggerMove();
        moveOnDeath2?.TriggerMove();
        deathSound?.PlayAndDetach();
        GetComponent<ResourceDrop>()?.Drop();
        Destroy(gameObject);
    }
}