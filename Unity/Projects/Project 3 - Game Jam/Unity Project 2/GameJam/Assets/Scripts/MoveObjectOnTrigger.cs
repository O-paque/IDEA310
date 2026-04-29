using UnityEngine;
using System.Collections;

public class MoveObjectOnTrigger : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;

    [Header("Movement")]
    [SerializeField] private Vector3 moveOffset;
    [SerializeField] private Vector3 targetPosition;
    [SerializeField] private bool useOffset = true;

    [Header("Timing")]
    [SerializeField] private float moveDuration = 1f;

    [Header("Trigger")]
    [SerializeField] private bool useTrigger = false;
    [SerializeField] private string triggerTag = "Player";

    [Header("Behavior")]
    [SerializeField] private bool moveOnlyOnce = true;

    private bool hasMoved = false;
    private Coroutine moveCoroutine;

    public void TriggerMove()
    {
        if (moveOnlyOnce && hasMoved)
            return;

        if (target == null)
        {
            Debug.LogWarning("MoveObjectOnTrigger: Target not assigned.");
            return;
        }

        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);

        moveCoroutine = StartCoroutine(MoveRoutine());
        hasMoved = true;
    }

    private IEnumerator MoveRoutine()
    {
        Vector3 startPosition = target.position;

        Vector3 endPosition = useOffset
            ? startPosition + moveOffset
            : targetPosition;

        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / moveDuration;

            target.position = Vector3.Lerp(startPosition, endPosition, t);
            yield return null;
        }

        target.position = endPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!useTrigger)
            return;

        if (!other.CompareTag(triggerTag))
            return;

        TriggerMove();
    }
}