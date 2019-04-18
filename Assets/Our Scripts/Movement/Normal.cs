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
        agent.GetComponent<NavMeshAgent>().isStopped = false;
        agent.GetComponent<MovementManager>().target = targetEnemy;
        
    }

    public override void Update()
    {
        
        if (targetEnemy != null)
        {
          
            agent.destination = targetEnemy.transform.position;
            agent.stoppingDistance = unitStast.range;
        }
        else
        {

            agent.destination = targetPosition;
            agent.stoppingDistance = 0f;
        }
    }

}
