using UnityEngine;
using System.Collections;

public class SwordBehavior : MonoBehaviour
{
    [Header("Swing Settings")]
    [SerializeField] private float swingDuration = 0.2f;
    [SerializeField] private float returnDuration = 0.15f;
    [SerializeField] private Vector3 swingRotation = new Vector3(0f, 0f, -80f);

    [Header("Damage Settings")]
    [SerializeField] private int damage = 25;
    [SerializeField] private float attackRange = 2.0f;
    [SerializeField] private LayerMask enemyLayers;

    private Quaternion startLocalRotation;
    private bool isSwinging = false;

    private void Start()
    {
        startLocalRotation = transform.localRotation;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isSwinging)
        {
            StartCoroutine(SwingSword());
        }
    }

    private IEnumerator SwingSword()
    {
        isSwinging = true;

        Quaternion targetRotation = startLocalRotation * Quaternion.Euler(swingRotation);

        float elapsed = 0f;

        while (elapsed < swingDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / swingDuration;
            transform.localRotation = Quaternion.Slerp(startLocalRotation, targetRotation, t);
            yield return null;
        }

        DoDamage();

        elapsed = 0f;

        while (elapsed < returnDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / returnDuration;
            transform.localRotation = Quaternion.Slerp(targetRotation, startLocalRotation, t);
            yield return null;
        }

        transform.localRotation = startLocalRotation;
        isSwinging = false;
    }

    private void DoDamage()
    {
        Debug.Log("DoDamage reached");

        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, attackRange))
        {
            Debug.Log("Raycast hit: " + hit.collider.name);

            if (hit.collider.CompareTag("Enemy"))
            {
                EnemyHealth enemyHealth = hit.collider.GetComponentInParent<EnemyHealth>();

                if (enemyHealth != null)
                {
                    Debug.Log($"Hit {hit.collider.name} for {damage} damage!");
                    enemyHealth.TakeDamage(damage);
                }
                else
                {
                    Debug.Log("Enemy tag found, but no EnemyHealth script");
                }
            }
            else
            {
                Debug.Log("Hit object is not tagged as Enemy");
            }
        }
        else
        {
            Debug.Log("Raycast did not hit anything");
        }
    }
}
