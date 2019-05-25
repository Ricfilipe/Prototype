using MalbersAnimations;
using MalbersAnimations.HAP;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHorse : Enemies
{

    private Animal animal;
    private NavMeshAgent agent;

    public float ToTrot = 1f;
    public float ToRun = 1.5f;

    protected override void Start()
    {
        animal = GetComponentInChildren<Animal>();
        agent = GetComponentInChildren<NavMeshAgent>();
        detectUnitsRadius = 70.0f;
        base.Start();
    }

    protected override void EnemyMovement()
    {
        agent.transform.localPosition = new Vector3(0f, 0f, 0f);
        if (marching || nearestUnit == null)
            {

                attacking = false;
                marching = true;
                enemyInAction.SetDestination(_targetWaypoint);
                DetectTargetingUnits();
            }
            else
            {
                enemyInAction.destination = (nearestUnit.gameObject.GetComponentInParent<UnitStats>().GetComponentInChildren<NavMeshAgent>().transform.position);

                enemyInAction.stoppingDistance = unitStats.range;
                Attack();
            }


       
        if (!agent.isStopped)
        {
            
            float RemainingDistance = (agent.transform.position - agent.destination).magnitude;

            if (RemainingDistance > 1.2f)
            {
                animal.Move(agent.desiredVelocity);
            }
            else
            {
                agent.isStopped = true;
                animal.Move(new Vector3(0, 0, 0));
            }

            if (RemainingDistance < ToTrot)         //Set to Walk
            {
                animal.Speed1 = true;
                animal.Speed2 = false;
                animal.Speed3 = false;
            }
            else if (RemainingDistance < ToRun)     //Set to Trot
            {
                animal.Speed1 = false;
                animal.Speed2 = true;
                animal.Speed3 = false;
            }
            else if (RemainingDistance > ToRun)     //Set to Run
            {
                animal.Speed1 = false;
                animal.Speed2 = false;
                animal.Speed3 = true;
            }

        }
        else
        {
            animal.Move(new Vector3(0, 0, 0));
        }
    }

    protected override void DetectTargetingUnits()
    {
        center = new Vector3(agent.transform.position.x, 0, agent.transform.position.z);
        base.DetectTargetingUnits();
    }

    protected override void Attack()
    {


        Vector3 helper = nearestUnit.gameObject.GetComponentInParent<UnitStats>().GetComponentInChildren<NavMeshAgent>().transform.position;
        helper = new Vector3(helper.x, 0, helper.z);
        Vector3 helper2 = new Vector3(agent.transform.position.x, 0, agent.transform.position.z);
   
        float updateDistance = Vector3.Distance(helper2, helper);

        if (updateDistance <= unitStats.range)
        {
            GetComponentInChildren<RiderCombat>().MainAttack();
        }
        else
        {
                //idle
        }

    }

    public void doDamage()
    {
        nearestUnit.GetComponentInParent<UnitStats>().HP -= GetComponentInParent<UnitStats>().getAD();
    }


    public override bool checkAlive()
    {
        return GetComponentInChildren<DeathHorse>().state == DeathHorse.State.Alive;
    }

    public override void prepareToDie()
    {
        if (gameObject.GetComponent<UnitStats>().HP <= 0)
        {
            gm.GetComponent<GameManager>().enemyPool.Remove(gameObject);
            StartCoroutine(GetComponentInChildren<DeathHorse>().Dead());
        }
    }
}
