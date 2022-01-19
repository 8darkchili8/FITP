using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyAI : MonoBehaviour
{
    //test poursuite grenade
    
    //test state
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    public LayerMask whatIsPlayer;
    public Transform player;

    public NavMeshAgent agent;
    public Transform[] waypoints;
    int waypointIndex;

    //Patrol
    public Vector3 target;
    bool walkPointSet;

    //Animator
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        UpdateDestination();
    }

    private void Awake()
    {
        player = GameObject.Find("grenade_lowpoly_prefab").transform; 
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange)
        {
            //animator.SetBool("SeePlayer", false);
            patrol();
        }
        if (playerInSightRange && !playerInAttackRange) 
        {
            agent.speed = 10f;
            animator.SetBool("SeePlayer", true);
            Chase();
        }
        if (playerInAttackRange && playerInSightRange)
        {
            animator.SetBool("AttackPlayer", true);
            attack();

        }
        //if (playerInSightRange) Chase();

    }
    void UpdateDestination()
    {
        target = waypoints[waypointIndex].position;
        agent.SetDestination(target);
    }

    void IterateWaypointIndex()
    {
        waypointIndex++;
        if (waypointIndex == waypoints.Length)
        {
            waypointIndex = 0;
        }
    }

    void Chase()
    {
        agent.SetDestination(player.position);
    }
    void patrol()
    {
        if (Vector3.Distance(transform.position, target) < 1)
        {
            IterateWaypointIndex();
            UpdateDestination();
        }
    }

    void attack()
    {
       
       agent.speed = 10f;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
