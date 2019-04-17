// MoveToClickPoint.cs
using UnityEngine;
using UnityEngine.AI;

public class MovementManager : MonoBehaviour
{

    public enum Type
    {
        King,
        Archer,
        MenAtArms
    }


    [SerializeField]
    public Camera cam;
    public Type type;
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