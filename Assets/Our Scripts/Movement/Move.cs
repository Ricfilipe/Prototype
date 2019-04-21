using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Move : UnitAction
{
    public Move(GameObject enemy, GameObject go, GameManager gm) : base(enemy, go, gm)
    {
    }

    public Move(Vector3 targetPos, GameObject go, GameManager gm) : base(targetPos, go, gm)
    {
    }

    public override void Start()
    {
        agent.GetComponent<NavMeshAgent>().isStopped = false;
        agent.GetComponent<MovementManager>().target = null;
    }

    public override void Update()
    {
        if (targetEnemy != null || !targetEnemy.GetComponent<UnitStats>().dead)
        {

            agent.destination = targetEnemy.transform.position;
         
            agent.stoppingDistance = 0f;
        }
        else if (targetPosition.y != -1000)
        {

            agent.destination = targetPosition;
            agent.stoppingDistance = 0f;
        }
        else
        {
            agent.isStopped = true;
        }
    }

}
