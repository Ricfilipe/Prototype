// MoveToClickPoint.cs
using UnityEngine;
using UnityEngine.AI;

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
    public bool selected;
    [HideInInspector]
    public GameObject target;
    private int attackCounter = 0;
    float attackRange;
    bool attack = false;
    int counter = 0;

    float attackTimer = 0;
    bool attacking;



    void Start()
    {
        this.gm = GameObject.FindGameObjectsWithTag("GameManager")[0];
        gm.GetComponent<GameManager>().myCharacterPool.Add(gameObject); 

        this.myUnitStats = GetComponent<UnitStats>();
        attackRange = myUnitStats.range;
    }




    void Update()
    {
        GetComponent<NavMeshAgent>().speed = myUnitStats.getSpeed();
        if (selected)
        {
            GetComponent<Outline>().enabled = true;
        }
        else
        {
            GetComponent<Outline>().enabled = false;
        }

        if (currentAction != null)
        {
            currentAction.Update();
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
                attackTimer = 1.5f;
                //Debug.Log(this.name + "\n" + target.gameObject.GetComponent<UnitStats>().HP + "\n" + target.gameObject.GetComponent<UnitStats>().getMaxHP());
            }

            if (target.gameObject.GetComponent<UnitStats>().HP <= 0)
            {
               gm.GetComponent<GameManager>().enemyPool.Remove(target.transform.parent.gameObject);
                target.GetComponent<Enemies>().DropSilver();
                Destroy(target.transform.parent.gameObject);
                target = null;
                attacking = false;
                
            }
        }
    }
}