using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNav : MonoBehaviour
{
    private NavMeshAgent agent;
    public Transform player;
    private Entity entity;
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
        entity = GetComponent<Entity>();
        agent = GetComponent<NavMeshAgent>();
        plr = GameObject.Find("Player");
        plrScript = plr.GetComponent<PlayerMovement>();
        player = plr.transform;
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
            //Debug.Log("!insight !inattack");
        }
        else if (playerInSightRange && !playerInAttackRange)
        {
            ChasePlayer();
            //Debug.Log("insight !inattack");
        }
        else if (playerInSightRange && playerInAttackRange) AttackPlayer();
        agent.SetDestination(player.position);

        if(entity.isDead)
        {
            agent.SetDestination(transform.position);
        }
    }

    void ChasePlayer()
    {
        agent.SetDestination(player.transform.position);
    }

    void AttackPlayer()
    {
        if (!attackCD && !entity.isDead)
        {
            plrScript.rb.AddExplosionForce(3000, transform.position, 25, 1.5f, ForceMode.Impulse);
            plrScript.Damaged(damage); //this gives an error msg in playmode despite nothing going wrong for some reason, just ignore it
            attackCD = true;
            Invoke(nameof(ResetAttack), attackInterval);
        }
    }

    void ResetAttack()
    {
        attackCD = false;
    }
}
