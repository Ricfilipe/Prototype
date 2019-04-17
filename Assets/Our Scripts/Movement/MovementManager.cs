// MoveToClickPoint.cs
using UnityEngine;
using UnityEngine.AI;

public class MovementManager : MonoBehaviour
{


    [SerializeField]
    public Camera cam;
    private UnitStats myUnitStats;
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


    void Start()
    {
        gm.GetComponent<GameManager>().myCharacterPool.Add(gameObject); 

        this.myUnitStats = gm.GetComponent<UnitStats>();
        gameObject.GetComponent<NavMeshAgent>().speed=myUnitStats.speed;
        attackRange = myUnitStats.range;
    }




    void Update()
    {

        if (selected)
        {
            GetComponent<Outline>().enabled = true;
        }
        else
        {
            GetComponent<Outline>().enabled = false;
        }

        if (currentAction != null) {
            currentAction.Update();
        }


       

        if (target!=null && (transform.position - target.transform.position).magnitude <= attackRange)
        {
            Debug.Log("Attacking");
            if (attackCounter >= 60)
            {
                Destroy(target);
                target = null;
                attackCounter = 0;
            }
            else { attackCounter++; }
        }

    }
}