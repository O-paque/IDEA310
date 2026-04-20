using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float force = 500f;
    private float timer = 0f, lifeTime = 5f;
    
    void Start()
    {
        var rb = GetComponent<Rigidbody>();
        rb.rotation = Quaternion.LookRotation(rb.linearVelocity);
        rb.AddForce(transform.forward * force);
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= lifeTime)
        {
            Destroy(gameObject);
        }
    }
}
