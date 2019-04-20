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

        this.myUnitStats = GetComponent<UnitStats>();
        attackRange = myUnitStats.range;
    }




    void Update()
    {
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

        if (currentAction != null)
        {
            currentAction.Update();
        }


        if (gameObject.GetComponent<UnitStats>().HP <= 0)
        {
            if (GetComponent<UnitStats>().troop == UnitStats.Troops.Infantry && !GetComponent<UnitStats>().undead)
            {
                StartCoroutine(GetComponent<MovementManager>().abs[0].DoAbility()); ;
            }

            if (!GetComponent<UnitStats>().undead)
            {
                gm.GetComponent<GameManager>().myCharacterPool.Remove(gameObject);
                gm.GetComponent<GameManager>().removeFromSelection(gameObject);
                Destroy(transform.parent.gameObject);

            }
        }



        Attack();
        
    }


    void Attack()
    {
        if (target != null)
        {
            if (attackTimer > 0)
            {
                attackTimer -= Time.deltaTime;
            }

            if ((transform.position - target.transform.position).magnitude <= attackRange)
            {
                attacking = true;
            }
            else
            {
                attacking = false;
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
                Destroy(target.transform.parent.gameObject);
                target = null;
                attacking = false;
                
            }
        }
    }

}