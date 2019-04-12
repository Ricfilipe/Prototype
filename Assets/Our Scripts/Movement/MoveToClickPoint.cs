// MoveToClickPoint.cs
using UnityEngine;
using UnityEngine.AI;

public class MoveToClickPoint : MonoBehaviour
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

    [HideInInspector]
    public bool selected;
    private GameObject target;
    private int attackCounter = 0;
    float attackRange;
    NavMeshAgent agent;
    bool attack = false;
    int counter = 0;

    void Start()
    {
        cam.GetComponent<SelectedArea>().myCharacterPool.Add(gameObject);
        agent = GetComponent<NavMeshAgent>();
        switch (type)
        {
            case Type.Archer:
                attackRange = 1.0f;
                break;
            case Type.MenAtArms:
                attackRange = 0.2f;
                break;
            case Type.King:
                attackRange = 0.2f;
                break;
        }

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemie")
        {
            counter = 0;
            attack = true;
            agent.Stop();
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Killed it");
        attack = false;
        
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Enemie")
        {
            counter++;
            attack = true;
            if (counter >= 100)
            {
                counter = 0;
                attack = false;
                Destroy(other.gameObject);
            }

        }
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

        

        if (Input.GetMouseButtonDown(1))
        {
    
            RaycastHit hit;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 300) && selected)
            {
                agent.isStopped = false;
                if (hit.collider.tag == "Enemie")
                {
                    if(target==null || target.Equals(hit.collider.gameObject))
                    {
                        counter = 0;
                    }
                    target = hit.collider.gameObject;
                    agent.destination = hit.collider.transform.position;
                    agent.stoppingDistance = attackRange;
                }
                else
                {
                    target = null;
                    agent.destination = hit.point;
                    agent.stoppingDistance = 0f;
                }
            }

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