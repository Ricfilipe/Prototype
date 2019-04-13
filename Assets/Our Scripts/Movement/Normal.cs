using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Normal : UnitAction
{
    public Normal(GameObject enemy, GameObject go) : base(enemy, go){}

    public Normal(Vector3 targetPos, GameObject go) : base(targetPos, go){}

    public override void Start()
    {
        throw new System.NotImplementedException();
    }

    public override void Update()
    {
        agent.isStopped = false;
        if (targetEnemy != null)
        {

            agent.destination = targetEnemy.transform.position;
            agent.stoppingDistance = 1.0f; //alterar isto para 
        }
        else
        {
           
            agent.destination = targetPosition;
            agent.stoppingDistance = 0f;
        }
    }

}
