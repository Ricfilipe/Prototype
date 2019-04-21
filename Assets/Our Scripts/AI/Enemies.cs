using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Enemies : MonoBehaviour
{

    [Header("Classes, Gameobjects and Nav")]
    public GameManager gm;
    protected NavMeshAgent enemyInAction;
    protected UnitStats unitStats;

    [Header("Troops movement inputs")]
    protected int movementSpeedRatio;
    protected bool attacking;
    protected bool marching;
    Vector3 _targetWaypoint;

    [Header("Troops attack inputs and stats")]
    protected float attackRange;
    protected float TotalHealth;
    protected float currentHealth;
    List<GameObject> playerTroops;
    protected Vector3 center;
    protected float detectUnitsRadius;
    protected float nearestDistance;
    protected GameObject nearestUnit;
    private float distance;
    private float updateDistance; 
    private float attackTimer = 0.25f;
    private float globalAttackTimer = 0;
    public GameObject weapon;

    [HideInInspector]
    public bool hover;

    [Header("Troops wandering")]
    private float timer;
    //public float wanderTimer;
    private int deg;
    private float degreeAngle;
    protected int ch = -1;

    [Header("Silver stats")]
    int silverDropped = 0;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        gm.enemyPool.Add(gameObject);
        unitStats = GetComponent<UnitStats>();
        marching = true;
        playerTroops = new List<GameObject>();
        enemyInAction = this.GetComponent<NavMeshAgent>();
        _targetWaypoint = new Vector3(180, 0, -10f);

    }

    // Update is called once per frame
    void Update()
    {
        if (globalAttackTimer > 0)
        {
            globalAttackTimer -= Time.deltaTime;
        }


        if (gameObject.GetComponent<UnitStats>().HP <= 0)
        {

            weapon.GetComponent<Animator>().Play("Idle");
            gm.GetComponent<GameManager>().enemyPool.Remove(gameObject);                
                StartCoroutine(GetComponent<Dying>().Dead());
        }

       if (GetComponent<Dying>().state == Dying.State.Alive) {
            if (nearestUnit != null)
            {
                if (nearestUnit.GetComponent<UnitStats>().dead)
                {
                    nearestUnit = null;
                }
            }
            EnemyMovement();

            if (hover)
            {
                GetComponent<Outline>().enabled = true;
                hover = false;
            }
            else
            {
                GetComponent<Outline>().enabled = false;
            }
        }
    }


    //Enemies wander, march, move to or away from player's army
    protected virtual void EnemyMovement()
    {
        if (marching || nearestUnit == null)
        {

            weapon.GetComponent<Animator>().Play("Idle");
            attacking = false;
            marching = true;
            enemyInAction.SetDestination(_targetWaypoint);
            DetectTargetingUnits();
        }
        else
        {
            enemyInAction.SetDestination(nearestUnit.transform.position);
            enemyInAction.stoppingDistance = unitStats.range;
            Attack();
        }

       /* 
        if (playerTroops.Count <= 0)
        {
            playerTroops = gm.myCharacterPool;
            ch = Random.Range(0, gm.myCharacterPool.Count);
            Debug.Log(playerTroops.Count);
            return;
        }
        else
        {
            if (!attacking)
            {
                 timer += Time.deltaTime;
                 if (timer >= wanderTimer)
                 {
                     deg = Random.Range(0, 16);
                     CalculateTargetPosition();
                     timer = 0;
                 }

                GameObject target = gm.myCharacterPool[ch];
                _targetWaypoint = new Vector3(target.transform.position.x, target.transform.position.y, target.transform.position.z);
                enemyInAction.SetDestination(_targetWaypoint);
            }
        }
        */
    }

    // attacks the units with cooldowns for each blow
    protected virtual void Attack()
    {
        
        transform.rotation = Quaternion.LookRotation((nearestUnit.transform.position - transform.position).normalized, Vector3.up);
        updateDistance = Vector3.Distance(transform.position, nearestUnit.transform.position);

        if (updateDistance <= unitStats.range)
        {
            attacking = true;
            if (globalAttackTimer <= 0)
            {

                if (attackTimer > 0)
                {
                    attackTimer -= Time.deltaTime;
                }
                weapon.GetComponent<Animator>().Play("attack");
            }
        }
        else
        {
            attacking = false;

            weapon.GetComponent<Animator>().Play("Idle");
        }

        if (attacking && attackTimer <= 0)
        {
            if (!nearestUnit.GetComponent<UnitStats>().undead)
            {
                nearestUnit.gameObject.GetComponent<UnitStats>().HP -= unitStats.AD;
            }
                attackTimer = 0.25f;
                globalAttackTimer = (GetComponent<UnitStats>().attackSpeed) - attackTimer;
                //Debug.Log(this.name + "\n" + nearestUnit.gameObject.GetComponent<UnitStats>().HP + "\n" + nearestUnit.gameObject.GetComponent<UnitStats>().MaxHP);
            
        }

    }

    //The enemy units are able to detect specifically player units, and search for the closest target to them to attack
    protected virtual void DetectTargetingUnits ()
    {
        nearestDistance = float.MaxValue;
        List<Collider> unitsCollided = new List<Collider>();
        List<Collider> currentUnitsInRange = new List<Collider>();
        unitsCollided.AddRange(Physics.OverlapSphere(center, detectUnitsRadius));
        foreach (Collider c in unitsCollided.FindAll(x => x.CompareTag("MyUnit")))
        {
            currentUnitsInRange.Add(c);
            distance = Vector3.Distance(center, c.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestUnit = c.gameObject;
            }
        }

        if (currentUnitsInRange.Count > 0 )
        {
            marching = false;
        }
        else
        {
            marching = true;
        }
    }

    //The enemy units when close enough charge (to be implemented #10 priority)
    protected virtual void Charge()
    {

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(center, detectUnitsRadius);
    }

    public void DropSilver()
    {
        if (silverDropped == 0)
        {
            gm.AddSilver(Random.Range(50, 100));
            silverDropped = 1;
        }
    }

    /*
    void CalculateTargetPosition()
    {
        degreeAngle = deg * 22.5f;
        float rad = degreeAngle * Mathf.Deg2Rad;
        _targetWaypoint = new Vector3(this.transform.position.x + distance.x, this.transform.position.y, this.transform.position.z + distance.z);
        _targetWaypoint = new Vector3(_targetWaypoint.x * Mathf.Cos(rad), _targetWaypoint.y, _targetWaypoint.z * Mathf.Sin(rad));
        Debug.Log(degreeAngle);
    }
    */
}
