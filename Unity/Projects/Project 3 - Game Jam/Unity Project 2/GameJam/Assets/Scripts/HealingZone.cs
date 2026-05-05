using UnityEngine;
using System.Collections;

public class HealingZone : MonoBehaviour
{
    [Header("Healing Settings")]
    [SerializeField] private int healAmount = 5;
    [SerializeField] private float healInterval = 1f;

    private Coroutine healCoroutine;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (healCoroutine == null)
        {
            healCoroutine = StartCoroutine(HealOverTime());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (healCoroutine != null)
        {
            StopCoroutine(healCoroutine);
            healCoroutine = null;
        }
    }

    private IEnumerator HealOverTime()
    {
        while (true)
        {
            if (HealthManager.Instance != null)
            {
                HealthManager.Instance.HealPlayer(healAmount);
            }

            yield return new WaitForSeconds(healInterval);
        }
    }
}
