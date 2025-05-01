using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class AiWorker : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform[] patrolPoints;
    private int currentPointIndex = 0;
    private Transform player;
    private bool chasingPlayer = false;
    private bool isAttacking = false;
    public float attackRange = 2f;
    public float attackCooldown = 2f;
    private EnemyAttack enemyAttack;
    private Animator animator;

    void Start()
    {
        if (patrolPoints.Length > 0)
        {
            agent.SetDestination(patrolPoints[currentPointIndex].position);
        }

        enemyAttack = GetComponent<EnemyAttack>();
        animator = GetComponentInChildren<Animator>(); // Make sure this targets the mesh
    }

    void Update()
    {
        if (chasingPlayer && player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer <= attackRange && !isAttacking)
            {
                StartCoroutine(AttackPlayer());
            }
            else
            {
                if (agent.isOnNavMesh)
                    agent.SetDestination(player.position);
            }
        }
        else if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            MoveToNextPoint();
        }
    }

    void MoveToNextPoint()
    {
        if (patrolPoints.Length == 0) return;

        currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
        if (agent.isOnNavMesh)
            agent.SetDestination(patrolPoints[currentPointIndex].position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            chasingPlayer = true;
            player = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            chasingPlayer = false;
            player = null;
            MoveToNextPoint();
        }
    }

    IEnumerator AttackPlayer()
    {
        isAttacking = true;
        agent.isStopped = true;

        // Play punch animation
        if (animator != null)
        {
            animator.SetTrigger("Punch");
        }

        //wait slightly so the animation has time to hit before damage
        yield return new WaitForSeconds(1f);

        if (enemyAttack != null && player != null)
        {
            enemyAttack.Attack(player.gameObject);
        }

        yield return new WaitForSeconds(attackCooldown);

        agent.isStopped = false;
        isAttacking = false;
    }
}
