using MalbersAnimations;
using MalbersAnimations.HAP;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HorseMovement : MovementManager
{
    private NavMeshAgent agent;
    public GameObject horse;
    public float ToTrot = 1f;
    public float ToRun = 1.5f;
    private Animal animal;
    private bool swordOut = false;

    // Start is called before the first frame update
    void Start()
    {
        attackRatio = 1;
        this.gm = GameObject.FindGameObjectsWithTag("GameManager")[0];
        gm.GetComponent<GameManager>().myCharacterPool.Add(gameObject);
        animal = GetComponentInChildren<Animal>();
        agent = GetComponentInChildren<NavMeshAgent>();
        agent.isStopped = true;
        this.myUnitStats = GetComponent<UnitStats>();
        
        
    }

    // Update is called once per frame
    void Update()
    {

        if (globalAttackTimer > 0)
        {
            globalAttackTimer -= Time.fixedDeltaTime;
        }




        if (GetComponentInChildren<DeathHorse>().state == DeathHorse.State.Alive)
        {
            if (currentAction != null)
            {
                currentAction.Update();
            }
            if (selected || hover)
            {
                GetComponent<Outline>().enabled = true;
                hover = false;
            }
            else
            {
                GetComponent<Outline>().enabled = false;
            }
            agent.transform.localPosition = new Vector3( 0 , 0,0);

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
                    animal.Move(new Vector3(0,0,0));
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

            if (currentAction != null)
            {
                currentAction.Update();
            }

            if (agent.isStopped && (target == null))
            {
                float min = myUnitStats.range;
                foreach (GameObject enemy in gm.GetComponent<GameManager>().enemyPool)
                {
                    float tempMin = (enemy.GetComponentInChildren<NavMeshAgent>().transform.position - GetComponentInChildren<NavMeshAgent>().transform.position).magnitude;
                   
                    if (tempMin <= min)
                    {
                        min = tempMin;
                        target = enemy;

                    }

                }
            }

            if (gameObject.GetComponentInParent<UnitStats>().HP <= 0)
            {
      
                    gm.GetComponent<GameManager>().myCharacterPool.Remove(gameObject);
                    gm.GetComponent<GameManager>().removeFromSelection(gameObject);
                    StartCoroutine(GetComponentInChildren<DeathHorse>().Dead());

                
        
            }



            Attack();
        }
    }

    void Attack()
    {
        if (target != null)
        {
           


            if ((agent.transform.position - target.transform.position).magnitude <= myUnitStats.range)
            {
                attacking = true;



                    if (!enchanced)
                    {
                        //normal attack
                        GetComponentInChildren<RiderCombat>().MainAttack();
                    }
                    else
                    {
                       //
                    }
                
            }
            else
            {
                //idle
  
            }




            if (target.gameObject.GetComponent<UnitStats>().HP <= 0)
            {
                gm.GetComponent<GameManager>().enemyPool.Remove(target);
                target.GetComponentInParent<Enemies>().DropSilver();
                target = null;
                attacking = false;

            }
        }
    }

    public void doDamage()
    {
        target.GetComponentInParent<UnitStats>().HP -= myUnitStats.getAD();
    }

}
