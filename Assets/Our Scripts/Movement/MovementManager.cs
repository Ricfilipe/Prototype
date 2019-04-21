// MoveToClickPoint.cs
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class MovementManager : MonoBehaviour
{


    [SerializeField]
    public Camera cam;
    private UnitStats myUnitStats;
    [HideInInspector]
    public GameObject gm;

    public GameObject bow;

    [HideInInspector]
    public UnitAction currentAction;
    [HideInInspector]
    public bool selected,hover;
    [HideInInspector]
    public GameObject target;
    private int attackCounter = 0;
    float attackRange;
    bool attack = false;
    int counter = 0;

    float attackTimer = 0;
    bool attacking;
    float attackRatio;

    public List<Ability> abs;


    void Start()
    {
        attackRatio = 1;
        this.gm = GameObject.FindGameObjectsWithTag("GameManager")[0];
        gm.GetComponent<GameManager>().myCharacterPool.Add(gameObject);
        GetComponent<NavMeshAgent>().isStopped = true;
        this.myUnitStats = GetComponent<UnitStats>();
        attackRange = myUnitStats.range;
    }




    void Update()
    {
        if (Input.GetKey(KeyCode.C) && selected){
            gm.GetComponent<GameManager>().myCharacterPool.Remove(gameObject);
            gm.GetComponent<GameManager>().removeFromSelection(gameObject);
            StartCoroutine(GetComponent<Dying>().Dead());
        }

        if (target != null)
        {
            if (target.GetComponent<UnitStats>().dead)
            {
                target = null;
            }
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

            if (attackTimer > 0)
            {
                attackTimer -= Time.deltaTime;
            }

            if ((transform.position - target.transform.position).magnitude <= attackRange)
            {
                attacking = true;
                if(myUnitStats.troop == UnitStats.Troops.Archer)
                    bow.GetComponent<Animator>().Play("anim_normal");
            }
            else
            {
                attacking = false;
                if (myUnitStats.troop == UnitStats.Troops.Archer)
                    bow.GetComponent<Animator>().Play("IdleBow");
            }

            if (attacking && attackTimer <= 0)
            {
                target.gameObject.GetComponent<UnitStats>().HP -= myUnitStats.getAD();
                attackTimer = myUnitStats.attackSpeed * attackRatio;
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