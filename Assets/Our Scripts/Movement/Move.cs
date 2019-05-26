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
        agent.GetComponentInChildren<NavMeshAgent>().isStopped = false;
        agent.GetComponentInParent<MovementManager>().target = null;
    }

    public override void Update()
    {
        if (targetEnemy != null )
        {

            if (!targetEnemy.GetComponentInParent<UnitStats>().dead)
            {
                agent.destination = targetEnemy.transform.position;
                Debug.Log(targetEnemy.name);
                agent.stoppingDistance = 0f;
            }
        }
        else if (targetPosition.y != -1000)
        {

            agent.destination = targetPosition;
            agent.stoppingDistance = 0f;
            var pos = agent.transform.position;
            pos.y = agent.destination.y;
            if (Vector3.Distance(agent.destination, pos) <= agent.stoppingDistance)
            {

                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {

                    agent.isStopped = true;

                }


            }
        }
        else
        {
            agent.isStopped = true;
        }
    }

}
