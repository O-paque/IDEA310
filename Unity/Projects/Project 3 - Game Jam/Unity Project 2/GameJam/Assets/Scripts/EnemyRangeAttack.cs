using UnityEngine;

public class EnemyRangedAttack : MonoBehaviour
{
    [Header("Projectile")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float projectileSpeed = 12f;

    [Header("Attack Settings")]
    [SerializeField] private float attackCooldown = 2f;

    [Header("Prediction")]
    [SerializeField] private bool usePredictiveAim = true;
    [SerializeField] private float maxPredictionTime = 2f;
    [SerializeField] private float aimHeightOffset = 1f;

    [Header("Optional Audio")]
    [SerializeField] private TriggeredSoundEffect attackSoundEffect;

    private float lastAttackTime = -Mathf.Infinity;

    public void Attack(GameObject player)
    {
        if (player == null || projectilePrefab == null || firePoint == null)
            return;

        if (Time.time < lastAttackTime + attackCooldown)
            return;

        Vector3 firePosition = firePoint.position;
        Vector3 targetPosition = player.transform.position + Vector3.up * aimHeightOffset;

        if (usePredictiveAim)
        {
            PlayerVelocityTracking velocityTracker = player.GetComponent<PlayerVelocityTracking>();

            if (velocityTracker != null)
            {
                Vector3 playerVelocity = velocityTracker.Velocity;
                targetPosition = GetPredictedAimPoint(firePosition, targetPosition, playerVelocity);
            }
        }

        Vector3 direction = (targetPosition - firePosition).normalized;

        GameObject projectileObject = Instantiate(
            projectilePrefab,
            firePosition,
            Quaternion.LookRotation(direction)
        );

        EnemyProjectile projectile = projectileObject.GetComponent<EnemyProjectile>();
        if (projectile != null)
        {
            projectile.Initialize(direction, projectileSpeed);
        }
        else
        {
            Debug.LogWarning("Projectile prefab is missing EnemyProjectile.");
        }

        if (attackSoundEffect != null)
            attackSoundEffect.PlaySound();

        lastAttackTime = Time.time;
    }

    private Vector3 GetPredictedAimPoint(Vector3 firePosition, Vector3 targetPosition, Vector3 targetVelocity)
    {
        Vector3 toTarget = targetPosition - firePosition;

        float a = Vector3.Dot(targetVelocity, targetVelocity) - (projectileSpeed * projectileSpeed);
        float b = 2f * Vector3.Dot(toTarget, targetVelocity);
        float c = Vector3.Dot(toTarget, toTarget);

        float discriminant = b * b - 4f * a * c;

        if (Mathf.Abs(a) < 0.001f || discriminant < 0f)
        {
            float fallbackTime = Vector3.Distance(firePosition, targetPosition) / projectileSpeed;
            fallbackTime = Mathf.Min(fallbackTime, maxPredictionTime);
            return targetPosition + targetVelocity * fallbackTime;
        }

        float sqrtDiscriminant = Mathf.Sqrt(discriminant);

        float t1 = (-b + sqrtDiscriminant) / (2f * a);
        float t2 = (-b - sqrtDiscriminant) / (2f * a);

        float interceptTime = Mathf.Min(t1, t2);

        if (interceptTime < 0f)
            interceptTime = Mathf.Max(t1, t2);

        if (interceptTime < 0f)
        {
            float fallbackTime = Vector3.Distance(firePosition, targetPosition) / projectileSpeed;
            fallbackTime = Mathf.Min(fallbackTime, maxPredictionTime);
            return targetPosition + targetVelocity * fallbackTime;
        }

        interceptTime = Mathf.Min(interceptTime, maxPredictionTime);

        return targetPosition + targetVelocity * interceptTime;
    }
}