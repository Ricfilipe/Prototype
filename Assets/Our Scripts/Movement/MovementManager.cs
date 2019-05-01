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

    public AudioSource[] spaningSound;

    [SerializeField]
    public Camera cam;
    protected UnitStats myUnitStats;
    [HideInInspector]
    public GameObject gm;

    public GameObject weapon;

    [HideInInspector]
    public UnitAction currentAction;
    [HideInInspector]
    public bool selected, hover;
    [HideInInspector]
    public GameObject target;
    protected int attackCounter = 0;
    protected float attackRange;
    protected bool attack = false;
    protected int counter = 0;

    protected bool footRight = true;

    protected float globalAttackTimer = 0;
    protected float attackTimer = 0.25f;
    protected bool attacking;
    protected float attackRatio;

    public List<Ability> abs;
    [HideInInspector]
    public bool enchanced;
    protected float timerStep;

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
            Vector3 helper = target.gameObject.GetComponentInParent<UnitStats>().transform.position;
            helper = new Vector3(helper.x, 0, helper.z);
            Vector3 helper2 = new Vector3(transform.position.x, 0, transform.position.z);
            transform.rotation = Quaternion.LookRotation((helper - helper2).normalized, Vector3.up);


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