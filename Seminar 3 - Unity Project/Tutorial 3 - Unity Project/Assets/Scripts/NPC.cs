using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class NPC : MonoBehaviour
{
    public enum NPCStates
    {
        Patrol,
        Chase,
        Attack
    }

    [SerializeField]
    Vector3[] PatrolPoints;
    [SerializeField]
    Transform Player;

    [SerializeField]
    Bullet Bullet;
    [SerializeField]
    Transform BulletSpawnPoint; //Use this as the potion to spawn your bullet

    [SerializeField]
    MeshRenderer StateMeshRenderer; //Use this to control coloured material for each state
    [SerializeField]
    Material PatrolMaterial;
    [SerializeField]
    Material ChaseMaterial;
    [SerializeField]
    Material AttackMaterial;

    [SerializeField]
    float ChaseRange = 7f;
    [SerializeField]
    float AttackRange = 4f;

    float fireRate = 2f;
    int nextPatrolPoint = 0;
    NPCStates currentState = NPCStates.Patrol;
    NavMeshAgent navMeshAgent;
    Animator animator;

    float nextShootTime = 0;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
       navMeshAgent.SetDestination(PatrolPoints[nextPatrolPoint]);
        animator = GetComponentInChildren<Animator>();

      

    }

    void Update()
    {
      
        SwitchState();
    }

    private void SwitchState()
    {        
        switch (currentState)
        {
            case NPCStates.Patrol:
                Patrol();
                break;
            case NPCStates.Chase:
                Chase();
                break;
            case NPCStates.Attack:
                Attack();
                break;
            default:
                Patrol();
                    break;
        }
    }

    private void Patrol() 
    {
        // Your code here

        //        If player is within chase range then change the state to chase.
        if (Vector3.Distance(this.transform.position, Player.transform.position) <= ChaseRange)
        {
            currentState= NPCStates.Chase;
            Debug.Log("PAtrol -> Chase");
        }

        //If the NPC is not moving:
        //Change the material to patrol material.
        StateMeshRenderer.material = PatrolMaterial;

        //Increment the next patrol point counter.
       nextPatrolPoint++;

        //If the next patrol point counter is >= the total number of patrol points:
        if(nextPatrolPoint >= PatrolPoints.Length)
        {  //        Set the next patrol point to zero.
            nextPatrolPoint = 0;
        }
      
        //Set the navmesh agent's destination to the next patrol point.

       navMeshAgent.destination = PatrolPoints[nextPatrolPoint];
    }    

    private void Chase()
    {
        // Your code here
        navMeshAgent.destination = Player.transform.position;
        StateMeshRenderer.material = ChaseMaterial;


        //Attack Range
        if (Vector3.Distance(this.transform.position, Player.transform.position) <= AttackRange)
        {
            currentState = NPCStates.Attack;
            Debug.Log("PAtrol -> Atack");
        }

        //Out of chase range
        if (Vector3.Distance(this.transform.position, Player.transform.position) > ChaseRange)
        {
            navMeshAgent.ResetPath();

            currentState = NPCStates.Patrol;
            Debug.Log("Chase -> Patrol");
        }




    }

    private void Attack() 
    {// Your code here
        navMeshAgent.ResetPath();
        StateMeshRenderer.material = AttackMaterial;


        //look at player

        Vector3 direction = Player.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f); // Smooth rotation


        if (Time.deltaTime > nextShootTime)
        {
            Bullet = Instantiate(Bullet, BulletSpawnPoint.position, transform.rotation);
            Debug.Log("Shooting");
            Bullet.rigidBody.linearVelocity = Vector3.forward * Bullet.Speed;
            nextShootTime = Time.deltaTime + fireRate;
        }

        if (Vector3.Distance(this.transform.position, Player.transform.position) > AttackRange)
        {
            currentState = NPCStates.Chase;
            Debug.Log("Attack -> Chase");
        }

    }


}
