using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class SkeletonBehavior : MonoBehaviour
{
    private GameObject player;
    private NavMeshAgent agent;
    private Animator animator;

    [Header("Ranges")]
    [SerializeField] private float chaseRange = 20f;
    [SerializeField] private float stopChaseRange = 24f;
    [SerializeField] private float attackRange = 2.25f;
    [SerializeField] private float stopAttackRange = 2.75f;

    [Header("Patrol")]
    [SerializeField] private float patrolRadius = 8f;
    [SerializeField] private float patrolPointTolerance = 1.0f;
    [SerializeField] private float patrolPointSampleDistance = 5f;
    [SerializeField] private float patrolWaitTime = 2f;

    private bool isWaitingAtPatrolPoint = false;
    private float patrolWaitTimer = 0f;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 3.5f;
    [SerializeField] private float rotationSpeed = 10f;

    [Header("Attack")]
    [SerializeField] private float attackCooldown = 1.5f;
    [SerializeField] private float attackDuration = 0.9f;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRadius = 1.3f;
    [SerializeField] private int attackDamage = 15;
    [SerializeField] private LayerMask playerLayers;

    [Header("Debug/Random")]
    [SerializeField] private bool drawAttackGizmos = true;
    [SerializeField] private TriggeredSoundEffect attackSound;

    private bool isChasing = false;
    private bool isAttacking = false;
    private bool damageAppliedThisSwing = false;

    private float lastAttackTime = -Mathf.Infinity;
    private float attackEndTime = -Mathf.Infinity;

    private Vector3 patrolCenter;
    private Vector3 currentPatrolPoint;

    private static readonly int IsMovingHash = Animator.StringToHash("IsMoving");
    private static readonly int TriggerAttackHash = Animator.StringToHash("TriggerAttack");

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        patrolCenter = transform.position;

        agent.speed = moveSpeed;
        agent.stoppingDistance = 0.5f;
        agent.autoBraking = false;

        if (NavMesh.SamplePosition(transform.position, out NavMeshHit startHit, 5f, NavMesh.AllAreas))
            agent.Warp(startHit.position);

        ChooseNewPatrolPoint();
    }

    private void Update()
    {
        if (player == null)
        {
            UpdateAnimator();
            return;
        }

        float playerDistance = Vector3.Distance(transform.position, player.transform.position);

        HandleStateTransitions(playerDistance);

        if (isAttacking)
        {
            HandleAttack();
        }
        else if (isChasing)
        {
            HandleChase(playerDistance);
        }
        else
        {
            HandlePatrol();
        }

        UpdateAnimator();
    }

    private void HandleStateTransitions(float playerDistance)
    {
        if (!isChasing && playerDistance < chaseRange)
        {
            isChasing = true;
        }
        else if (isChasing && playerDistance > stopChaseRange && !isAttacking)
        {
            isChasing = false;
            isWaitingAtPatrolPoint = false;
            patrolWaitTimer = 0f;
            ChooseNewPatrolPoint();
        }

        if (isChasing && !isAttacking && playerDistance <= attackRange && Time.time >= lastAttackTime + attackCooldown)
        {
            StartAttack();
        }
    }

    private void HandlePatrol()
    {
        float arrivalDistance = Mathf.Max(patrolPointTolerance, agent.stoppingDistance);

        // If already waiting, count down and then go to the next point
        if (isWaitingAtPatrolPoint)
        {
            patrolWaitTimer -= Time.deltaTime;

            agent.ResetPath();

            if (patrolWaitTimer <= 0f)
            {
                isWaitingAtPatrolPoint = false;
                ChooseNewPatrolPoint();
                agent.SetDestination(currentPatrolPoint);
            }

            return;
        }

        // Keep moving toward current patrol point
        agent.SetDestination(currentPatrolPoint);

        // Once the path is ready and we're close enough, start waiting
        if (!agent.pathPending && agent.remainingDistance <= arrivalDistance)
        {
            isWaitingAtPatrolPoint = true;
            patrolWaitTimer = patrolWaitTime;
            agent.ResetPath();
        }
    }

    private void HandleChase(float playerDistance)
    {
        if (player == null)
            return;

        if (playerDistance <= stopAttackRange && Time.time >= lastAttackTime + attackCooldown)
        {
            StartAttack();
            return;
        }

        if (NavMesh.SamplePosition(player.transform.position, out NavMeshHit hit, 5f, NavMesh.AllAreas))
            agent.SetDestination(hit.position);
    }

    private void StartAttack()
    {
        isAttacking = true;
        damageAppliedThisSwing = false;
        lastAttackTime = Time.time;
        attackEndTime = Time.time + attackDuration;

        agent.ResetPath();
        FacePlayer();

        animator.SetBool(IsMovingHash, false);
        animator.SetTrigger(TriggerAttackHash);
    }

    private void HandleAttack()
    {
        agent.ResetPath();
        FacePlayer();

        if (Time.time >= attackEndTime)
        {
            isAttacking = false;
        }
    }

    private void FacePlayer()
    {
        if (player == null)
            return;

        Vector3 lookTarget = player.transform.position;
        lookTarget.y = transform.position.y;

        Vector3 direction = lookTarget - transform.position;
        if (direction.sqrMagnitude <= 0.001f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    private void ChooseNewPatrolPoint()
    {
        for (int i = 0; i < 10; i++)
        {
            Vector2 randomCircle = Random.insideUnitCircle * patrolRadius;
            Vector3 rawPoint = patrolCenter + new Vector3(randomCircle.x, 0f, randomCircle.y);

            if (NavMesh.SamplePosition(rawPoint, out NavMeshHit hit, patrolPointSampleDistance, NavMesh.AllAreas))
            {
                currentPatrolPoint = hit.position;
                return;
            }
        }

        currentPatrolPoint = patrolCenter;
    }

    private void UpdateAnimator()
    {
        if (animator == null || agent == null)
            return;

        bool moving = !isAttacking && agent.velocity.magnitude > 0.1f;
        animator.SetBool(IsMovingHash, moving);
    }

    public void ApplySwordDamage()
    {
        Debug.Log("ApplySwordDamage called");

        if (damageAppliedThisSwing)
            return;

        if (attackPoint == null)
        {
            Debug.LogWarning("SkeletonBehavior: attackPoint is not assigned.", this);
            return;
        }

        Collider[] hits = Physics.OverlapSphere(attackPoint.position, attackRadius);

        bool hitPlayer = false;

        for (int i = 0; i < hits.Length; i++)
        {
            Collider hit = hits[i];

            if (hit.CompareTag("Player") || hit.transform.root.CompareTag("Player"))
            {
                hitPlayer = true;
                break;
            }
        }

        if (hitPlayer)
        {
            if (HealthManager.Instance != null)
            {
                HealthManager.Instance.TakeDamage(attackDamage);
                Debug.Log($"Skeleton hit player for {attackDamage} damage.");
            }
            else
            {
                Debug.LogWarning("HealthManager.Instance is null.");
            }
        }
        else
        {
            Debug.Log("Skeleton swing missed.");
        }

        damageAppliedThisSwing = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stopChaseRange);

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        if (drawAttackGizmos && attackPoint != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
        }
    }

    public void PlayAttackSound()
    {
        attackSound?.PlaySound();
    }
}