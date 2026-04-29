using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class RangedEnemyBehavior : MonoBehaviour
{
    private GameObject player;
    private NavMeshAgent agent;

    [Header("Ranges")]
    [SerializeField] private float chaseRange = 80f;
    [SerializeField] private float stopChaseRange = 90f;
    [SerializeField] private float attackRange = 12f;

    [Header("Patrol")]
    [SerializeField] private float patrolRadius = 10f;
    [SerializeField] private float patrolPointTolerance = 1.25f;
    [SerializeField] private float patrolPointSampleDistance = 6f;
    [SerializeField] private float attackMovementRadius = 6f;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 3.5f;
    [SerializeField] private float lookRotationSpeed = 8f;

    [Header("Audio")]
    [SerializeField] private TriggeredSoundEffect chaseSoundEffect;
    [SerializeField] private TriggeredSoundEffect patrolSoundEffect;

    [Header("Combat")]
    [SerializeField] private EnemyRangedAttack rangedAttack;

    [Header("Floating Visual")]
    [SerializeField] private bool useFloatingVisual = false;
    [SerializeField] private Transform visualRoot;
    [SerializeField] private float bobHeight = 0.35f;
    [SerializeField] private float bobSpeed = 2f;

    private bool isChasing = false;
    private bool isAttacking = false;

    private Vector3 patrolCenter;
    private Vector3 currentPatrolPoint;
    private Vector3 visualStartLocalPos;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();

        patrolCenter = transform.position;
        agent.speed = moveSpeed;

        if (NavMesh.SamplePosition(transform.position, out NavMeshHit startHit, 5f, NavMesh.AllAreas))
        {
            agent.Warp(startHit.position);
        }

        if (useFloatingVisual && visualRoot != null)
            visualStartLocalPos = visualRoot.localPosition;

        ChooseNewPatrolPoint(patrolCenter, patrolRadius);

        if (patrolSoundEffect != null)
            patrolSoundEffect.StartLoop();
    }

    private void Update()
    {
        if (player == null || agent == null)
            return;

        float playerDistance = Vector3.Distance(transform.position, player.transform.position);

        HandleStateTransitions(playerDistance);

        if (!isChasing)
        {
            HandlePatrol();
        }
        else if (isAttacking)
        {
            HandleAttackMovement();
        }
        else
        {
            HandleChase();
        }

        if (isAttacking)
            LookAtPlayer();

        UpdateFloatingVisual();
    }

    private void HandleStateTransitions(float playerDistance)
    {
        if (!isChasing && playerDistance < chaseRange)
        {
            isChasing = true;

            if (patrolSoundEffect != null)
                patrolSoundEffect.StopLoop();

            if (chaseSoundEffect != null)
                chaseSoundEffect.PlaySound();
        }
        else if (isChasing && playerDistance > stopChaseRange)
        {
            isChasing = false;
            isAttacking = false;
            agent.updateRotation = true;

            ChooseNewPatrolPoint(patrolCenter, patrolRadius);

            if (patrolSoundEffect != null)
                patrolSoundEffect.StartLoop();
        }

        if (isChasing && playerDistance <= attackRange)
        {
            if (!isAttacking)
            {
                isAttacking = true;
                agent.updateRotation = false;
                ChooseNewPatrolPoint(transform.position, attackMovementRadius);
            }
        }
        else
        {
            if (isAttacking)
            {
                isAttacking = false;
                agent.updateRotation = true;
            }
        }
    }

    private void HandleChase()
    {
        if (player == null)
            return;

        if (NavMesh.SamplePosition(player.transform.position, out NavMeshHit hit, 5f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
            Debug.Log("Chasing player");
        }
    }

    private void HandleAttackMovement()
    {
        rangedAttack?.Attack(player);

        if (!agent.pathPending && agent.remainingDistance <= patrolPointTolerance)
        {
            ChooseNewPatrolPoint(transform.position, attackMovementRadius);
        }

        agent.SetDestination(currentPatrolPoint);
        Debug.Log("Attacking while moving randomly");
    }

    private void HandlePatrol()
    {
        if (!agent.pathPending && agent.remainingDistance <= patrolPointTolerance)
        {
            ChooseNewPatrolPoint(patrolCenter, patrolRadius);
        }

        agent.SetDestination(currentPatrolPoint);
        Debug.Log("Patrolling");
    }

    private void ChooseNewPatrolPoint(Vector3 center, float radius)
    {
        for (int i = 0; i < 10; i++)
        {
            Vector2 randomCircle = Random.insideUnitCircle * radius;
            Vector3 rawPoint = center + new Vector3(randomCircle.x, 0f, randomCircle.y);

            if (NavMesh.SamplePosition(rawPoint, out NavMeshHit hit, patrolPointSampleDistance, NavMesh.AllAreas))
            {
                currentPatrolPoint = hit.position;
                return;
            }
        }

        currentPatrolPoint = center;
    }

    private void LookAtPlayer()
    {
        if (player == null)
            return;

        Vector3 lookTarget = player.transform.position;
        lookTarget.y = transform.position.y;

        Vector3 direction = lookTarget - transform.position;
        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                lookRotationSpeed * Time.deltaTime
            );
        }
    }

    private void UpdateFloatingVisual()
    {
        if (!useFloatingVisual || visualRoot == null)
            return;

        Vector3 localPos = visualStartLocalPos;
        localPos.y += Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        visualRoot.localPosition = localPos;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stopChaseRange);

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}