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
    private GameObject targeting;

    protected override void Start()
    {
       
        agent = GetComponentInChildren<NavMeshAgent>();
        detectUnitsRadius = 20.0f;
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
                if (nearestUnit != null)
                {
                    agent.destination = (nearestUnit.gameObject.GetComponentInParent<UnitStats>().GetComponentInChildren<NavMeshAgent>().transform.position);

                    agent.stoppingDistance = unitStats.range;
                    Attack();
                }
                else
                {
                    if (targeting == null)
                    {

                        GameObject King = null;
                        float min = float.MaxValue;
                        foreach (GameObject archer in gm.myCharacterPool)
                        {

                            if (archer.GetComponentInChildren<UnitStats>().troop == UnitStats.Troops.Archer)
                            {
                                var tempMin = Vector3.Distance(transform.position, archer.GetComponentInChildren<NavMeshAgent>().transform.position);

                                if (tempMin < min)
                                {
                                    DetectTargetingUnits();
                                    min = tempMin;
                                    targeting = archer;
                                }
                            }
                            else if (archer.GetComponentInChildren<UnitStats>().troop == UnitStats.Troops.King)
                            {
                                King = archer;
                            }
                        }
                        if (targeting == null)
                        {
                            targeting = King;
                        }

                    }

                    agent.destination = targeting.GetComponentInChildren<NavMeshAgent>().transform.position;
                    DetectTargetingUnits();
                }
                

                
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
