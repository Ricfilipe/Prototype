using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Enemies : MonoBehaviour
{

    public GameManager gm;

    protected int movementSpeedRatio;
    protected NavMeshAgent enemyInAction;

    protected float attackRange;
    protected float Health;

    Vector3 firstWaypoint;

    protected bool attacking;

    [Header("Troops wandering")]
    private float timer;
    public float wanderTimer;
    private int deg;
    private float degreeAngle;
    private Vector3 distance;
    protected int ch = -1;

    List<GameObject> playerTroops;



    // Start is called before the first frame update
    protected virtual void Start()
    {
        playerTroops = new List<GameObject>();
        distance = new Vector3(200, 0, 200);
        enemyInAction = this.GetComponent<NavMeshAgent>();
        enemyInAction.speed = this.enemyInAction.speed * movementSpeedRatio;
        firstWaypoint = new Vector3(this.transform.position.x + distance.x, this.transform.position.y, this.transform.position.z + distance.z);
    }

    // Update is called once per frame
    void Update()
    {
        EnemyMovement();
    }


    //Enemies wander, march, move to or away from player's army
    protected virtual void EnemyMovement()
    {
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
                /* timer += Time.deltaTime;
                 if (timer >= wanderTimer)
                 {
                     deg = Random.Range(0, 16);
                     CalculateTargetPosition();
                     timer = 0;
                 }*/

                GameObject target = gm.myCharacterPool[ch];
                firstWaypoint = new Vector3(target.transform.position.x, target.transform.position.y, target.transform.position.z);
                enemyInAction.SetDestination(firstWaypoint);
            }
        }

    }

    protected virtual void Attack() { }

    void CalculateTargetPosition()
    {
        degreeAngle = deg * 22.5f;
        float rad = degreeAngle * Mathf.Deg2Rad;
        firstWaypoint = new Vector3(this.transform.position.x + distance.x, this.transform.position.y, this.transform.position.z + distance.z);
        firstWaypoint = new Vector3(firstWaypoint.x * Mathf.Cos(rad), firstWaypoint.y, firstWaypoint.z * Mathf.Sin(rad));
        Debug.Log(degreeAngle);
    }

}
