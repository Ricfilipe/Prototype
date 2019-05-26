using MalbersAnimations;
using MalbersAnimations.HAP;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHorse : Enemies
{

    private NavMeshAgent agent;

    public Vector3 firstMove;
    public float waitTimeBeforeAttack;
    private bool didFirst=true;

    protected override void Start()
    {
       
        agent = GetComponentInChildren<NavMeshAgent>();
        detectUnitsRadius = 70.0f;
        base.Start();
        agent.speed = unitStats.speed;
        firstMove.y = transform.position.y;
    }

    protected override void EnemyMovement()
    {
    

        if (!following)
        {
            agent.speed = unitStats.speed;
            if (didFirst)
            {
                agent.destination = firstMove;

            }
            else
            {

                base.EnemyMovement();

            }

            if ((transform.position - firstMove).magnitude < 1f )
            {
                agent.isStopped = true;
                if (waitTimeBeforeAttack > 0)
                {
                    waitTimeBeforeAttack -= Time.fixedDeltaTime;
                }
                else
                {
                    didFirst = false;
                    agent.isStopped = false;
                }
            }
        }
        else
        {
            didFirst = false;
            base.EnemyMovement();
        }

    }

    protected override void DetectTargetingUnits()
    {
        center = new Vector3(transform.position.x, 0, transform.position.z);
        base.DetectTargetingUnits();
    }

    protected override void Attack()
    {
        base.Attack();

    }

}
