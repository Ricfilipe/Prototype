using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentCollision : MonoBehaviour
{
    // Start is called before the first frame update

    NavMeshAgent agent;
    bool attack = false;

    void Start()
    {
        agent = GetComponentInParent<NavMeshAgent>();
    }


    private void OnTriggerEnter(Collider other)
    {
        
        attack = true;
        agent.Stop();
        Debug.Log("WTF");
    }

    private void OnTriggerExit(Collider other)
    {
        attack = false;
    }
}