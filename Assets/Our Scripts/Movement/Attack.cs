using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Attack : UnitAction
{
    public Attack(GameObject enemy, GameObject go, GameManager gm) : base(enemy, go, gm)
    {
    }

    public Attack(Vector3 targetPos, GameObject go,GameManager gm) : base(targetPos, go,gm)
    {
    }

    

    public override void Start()
    {
       
        agent.GetComponent<NavMeshAgent>().isStopped = false;
        if (targetEnemy == null)
        {
            agent.destination = targetPosition;
            agent.stoppingDistance = 0f;
        }
        else
        {
            Debug.Log(targetEnemy);
            agent.GetComponent<MovementManager>().target = targetEnemy;
        }
    }

    public override void Update()
    {
        Debug.Log(targetEnemy);
        if (targetEnemy == null)
        {
            float min= 60f;
            foreach (GameObject enemy in gm.enemyPool)
            {
                float tempMin = (enemy.transform.position - agent.transform.position).magnitude;
                if (tempMin <= min)
                {
                    min = tempMin;
                    targetEnemy = enemy;
                    
                }

            }


        }
        if (targetEnemy != null)
        {
            agent.GetComponent<MovementManager>().target = targetEnemy;
            agent.destination = targetEnemy.transform.position;
            agent.stoppingDistance = unitStast.range;
        }
    }

}
