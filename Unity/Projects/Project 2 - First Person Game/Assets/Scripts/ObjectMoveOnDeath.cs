using UnityEngine;
using System.Collections;

public class ObjectMoveOnDeath : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;

    [Header("Movement")]
    [SerializeField] private bool useWorldTargetPosition = false;
    [SerializeField] private Vector3 moveOffset;
    [SerializeField] private Vector3 targetPosition;

    [Header("Timing")]
    [SerializeField] private float moveDuration = 1f;
    [SerializeField] private float delayBeforeMove = 0f;

    [Header("Behavior")]
    [SerializeField] private bool triggerOnlyOnce = true;

    private bool hasTriggered = false;
    private Coroutine moveCoroutine;

    public void TriggerMove()
    {
        if (triggerOnlyOnce && hasTriggered)
            return;

        if (target == null)
        {
            Debug.LogWarning("MoveObjectOnEnemyDeath: target is not assigned.", this);
            return;
        }

        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);

        moveCoroutine = StartCoroutine(MoveRoutine());
        hasTriggered = true;
    }

    private IEnumerator MoveRoutine()
    {
        if (delayBeforeMove > 0f)
            yield return new WaitForSeconds(delayBeforeMove);

        Vector3 startPosition = target.position;
        Vector3 endPosition = useWorldTargetPosition
            ? targetPosition
            : startPosition + moveOffset;

        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / moveDuration);
            t = Mathf.SmoothStep(0f, 1f, t);

            target.position = Vector3.Lerp(startPosition, endPosition, t);
            yield return null;
        }

        target.position = endPosition;
        moveCoroutine = null;
    }
}
