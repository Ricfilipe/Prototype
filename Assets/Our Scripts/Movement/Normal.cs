using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Normal : UnitAction
{
    public Normal(GameObject enemy, GameObject go, GameManager gm) : base(enemy, go, gm) { }

    public Normal(Vector3 targetPos, GameObject go, GameManager gm) : base(targetPos, go, gm) { }

    public override void Start()
    {
        agent.isStopped = false;
        agent.GetComponentInParent<MovementManager>().target = targetEnemy;
        
    }

    public override void Update()
    {
        
        if (targetEnemy != null)
        {
            if (!targetEnemy.GetComponentInParent<UnitStats>().dead)
            {

                agent.destination = targetEnemy.transform.position;
                agent.stoppingDistance = unitStast.range;
            }
        }
        else if (targetPosition.y != -1000)
        {
          
            agent.destination = targetPosition;
            agent.stoppingDistance = 0f;
            var pos = agent.transform.position;
            pos.y = agent.destination.y;
            if (Vector3.Distance(agent.destination,pos)<= agent.stoppingDistance)
            {

                if(!agent.hasPath ||agent.velocity.sqrMagnitude == 0f)
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
