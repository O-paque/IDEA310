using UnityEngine;
using System.Collections;

public class EnemySwordAttack : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform sword;

    [Header("Attack Settings")]
    [SerializeField] private float attackRange = 2.0f;
    [SerializeField] private float attackCooldown = 1.5f;
    [SerializeField] private int damage = 10;

    [Header("Swing Settings")]
    [SerializeField] private float swingDuration = 0.2f;
    [SerializeField] private float returnDuration = 0.15f;
    [SerializeField] private Vector3 swingRotation = new Vector3(0f, 0f, -80f);

    private Quaternion swordStartLocalRotation;
    private bool isAttacking = false;
    private float cooldownTimer = 0f;

    private void Start()
    {
        if (sword != null)
        {
            swordStartLocalRotation = sword.localRotation;
        }

        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
        }
    }

    private void Update()
    {
        if (player == null || sword == null)
        {
            return;
        }

        cooldownTimer -= Time.deltaTime;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange && cooldownTimer <= 0f && !isAttacking)
        {
            StartCoroutine(Attack());
        }
    }

    private IEnumerator Attack()
    {
        isAttacking = true;
        cooldownTimer = attackCooldown;

        Quaternion targetRotation = swordStartLocalRotation * Quaternion.Euler(swingRotation);

        float elapsed = 0f;

        while (elapsed < swingDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / swingDuration;
            sword.localRotation = Quaternion.Slerp(swordStartLocalRotation, targetRotation, t);
            yield return null;
        }

        DealDamageToPlayer();

        elapsed = 0f;

        while (elapsed < returnDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / returnDuration;
            sword.localRotation = Quaternion.Slerp(targetRotation, swordStartLocalRotation, t);
            yield return null;
        }

        sword.localRotation = swordStartLocalRotation;
        isAttacking = false;
    }

    private void DealDamageToPlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange && HealthManager.Instance != null)
        {
            HealthManager.Instance.TakeDamage(damage);
        }
    }
}