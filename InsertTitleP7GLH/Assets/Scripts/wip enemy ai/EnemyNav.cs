using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNav : MonoBehaviour
{
    private NavMeshAgent agent;
    public Transform player;
    private GameObject plr;
    private PlayerMovement plrScript;

    public LayerMask whatIsGround, whatIsPlayer;

    private Vector3 walkPoint;
    private bool walkPointSet;
    public float walkPointRange;

    public float attackInterval;
    bool attackCD;
    public float damage;

    public float sightRange, attackRange;
    private bool playerInSightRange, playerInAttackRange;


    // Start is called before the first frame update
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        plr = GameObject.Find("Player");
        plrScript = plr.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        //enemy stays in place if player is too far away, chases when close, & attacks/flings when close enough to attack
        if (!playerInSightRange && !playerInAttackRange)
        {
            agent.SetDestination(transform.position);
            Debug.Log("!insight !inattack");
        }
        else if (playerInSightRange && !playerInAttackRange)
        {
            ChasePlayer();
            Debug.Log("insight !inattack");
        }
        else if (playerInSightRange && playerInAttackRange) AttackPlayer();
        agent.SetDestination(player.position);
    }

    void ChasePlayer()
    {
        agent.SetDestination(player.transform.position);
    }

    void AttackPlayer()
    {
        if (!attackCD)
        {
            plrScript.rb.AddExplosionForce(800, transform.position, 25, 1.2f, ForceMode.Impulse);
            plrScript.Damaged(damage, transform);
            attackCD = true;
            Invoke(nameof(ResetAttack), attackInterval);
        }
    }

    void ResetAttack()
    {
        attackCD = false;
    }
}
