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

    void Start()
    {
        if (patrolPoints.Length > 0)
        {
            agent.SetDestination(patrolPoints[currentPointIndex].position);
        }
        enemyAttack = GetComponent<EnemyAttack>(); // Ensure the attack script is attached
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
        agent.isStopped = true; // Stop moving to attack

        if (enemyAttack != null)
        {
            enemyAttack.Attack(player.gameObject); // Calls the attack function
        }

        yield return new WaitForSeconds(attackCooldown); // Wait before attacking again

        agent.isStopped = false;
        isAttacking = false;
    }
}
