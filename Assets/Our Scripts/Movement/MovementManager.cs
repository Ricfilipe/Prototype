// MoveToClickPoint.cs
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class MovementManager : MonoBehaviour
{



    public AudioSource[] movingSpecific;
    public AudioSource[] attackSpecific;
    public AudioSource[] movingGeneric;
    public AudioSource[] attackingGeneric;



    [SerializeField]
    public Camera cam;
    private UnitStats myUnitStats;
    [HideInInspector]
    public GameObject gm;

    public GameObject weapon;

    [HideInInspector]
    public UnitAction currentAction;
    [HideInInspector]
    public bool selected, hover;
    [HideInInspector]
    public GameObject target;
    private int attackCounter = 0;
    float attackRange;
    bool attack = false;
    int counter = 0;

    bool footRight = true;

    float globalAttackTimer = 0;
    float attackTimer = 0.25f;
    bool attacking;
    float attackRatio;

    public List<Ability> abs;
    [HideInInspector]
    public bool enchanced;
    private float timerStep;

    void Start()
    {
        attackRatio = 1;
        this.gm = GameObject.FindGameObjectsWithTag("GameManager")[0];
        gm.GetComponent<GameManager>().myCharacterPool.Add(gameObject);
        GetComponent<NavMeshAgent>().isStopped = true;
        this.myUnitStats = GetComponent<UnitStats>();
        
    }




    void Update()
    {


        if (globalAttackTimer > 0)
        {
            globalAttackTimer -= Time.fixedDeltaTime;
        }

        if (target != null)
        {
            if (target.GetComponent<UnitStats>().dead)
            {
                weapon.GetComponent<Animator>().Play("Idle");
                target = null;
            }
        }
        else
        {
            weapon.GetComponent<Animator>().Play("Idle");
        }

        GetComponent<NavMeshAgent>().speed = myUnitStats.getSpeed();
        if (selected||hover)
        {
            GetComponent<Outline>().enabled = true;
            hover = false;
        }
        else
        {
            GetComponent<Outline>().enabled = false;
        }


        if (GetComponent<Dying>().state == Dying.State.Alive)
        {

            if (currentAction != null)
            {
                currentAction.Update();
            }

            if (GetComponent<NavMeshAgent>().isStopped && (target == null))
            {
                float min = myUnitStats.range;
                foreach (GameObject enemy in gm.GetComponent<GameManager>().enemyPool)
                {
                    float tempMin = (enemy.transform.position - transform.position).magnitude;
                    if (tempMin <= min)
                    {
                        min = tempMin;
                        target = enemy;

                    }

                }
            }


            if (gameObject.GetComponent<UnitStats>().HP <= 0)
            {
                if (GetComponent<UnitStats>().troop == UnitStats.Troops.Infantry && !GetComponent<UnitStats>().undead)
                {
                    StartCoroutine(GetComponent<MovementManager>().abs[0].DoAbility());
                }

                if (!GetComponent<UnitStats>().undead)
                {
                    weapon.GetComponent<Animator>().Play("Idle");
                    gm.GetComponent<GameManager>().myCharacterPool.Remove(gameObject);
                    gm.GetComponent<GameManager>().removeFromSelection(gameObject);
                    StartCoroutine(GetComponent<Dying>().Dead());

                }
            }



            Attack();
        }
    }


    void Attack()
    {
        if (target != null)
        {
            transform.rotation = Quaternion.LookRotation((target.transform.position - transform.position).normalized,Vector3.up);


            if ((transform.position - target.transform.position).magnitude <= myUnitStats.range)
            {
                attacking = true;
                if (globalAttackTimer <= 0)
                {

                    if (attackTimer > 0)
                    {
                        attackTimer -= Time.fixedDeltaTime;
                    }
                    if (!enchanced)
                    {
                        weapon.GetComponent<Animator>().Play("attack");
                    }
                    else
                    {
                        weapon.GetComponent<Animator>().Play("attack_en");
                    }
                }
            }
            else
            {
                attacking = false;
               
                    weapon.GetComponent<Animator>().Play("Idle");
            }

            if (attacking && attackTimer <= 0)
            {

                
                attackTimer = 0.25f;
                if (enchanced)
                {
                    attackTimer = 0.125f; 
                }
                globalAttackTimer = (myUnitStats.attackSpeed )-attackTimer;
                
                //Debug.Log(this.name + "\n" + target.gameObject.GetComponent<UnitStats>().HP + "\n" + target.gameObject.GetComponent<UnitStats>().getMaxHP());
            }


            if (target.gameObject.GetComponent<UnitStats>().HP <= 0)
            {
               gm.GetComponent<GameManager>().enemyPool.Remove(target);
                target.GetComponent<Enemies>().DropSilver();
                target = null;
                attacking = false;
                
            }
        }
    }



}