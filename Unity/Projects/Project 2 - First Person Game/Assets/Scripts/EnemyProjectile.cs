using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] private float lifetime = 5f;
    [SerializeField] private int damage = 10;

    private Vector3 moveDirection;
    private float speed;

    public void Initialize(Vector3 direction, float projectileSpeed)
    {
        moveDirection = direction.normalized;
        speed = projectileSpeed;
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        transform.position += moveDirection * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
            return;

        if (other.CompareTag("Player") && HealthManager.Instance != null)
        {
            Debug.Log($"Projectile hit player for {damage} damage.");
            HealthManager.Instance.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}