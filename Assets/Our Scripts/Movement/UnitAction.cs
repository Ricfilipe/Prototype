using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class UnitAction 
{
    protected Vector3 targetPosition;
    protected GameObject targetEnemy;
    protected NavMeshAgent agent;
    protected UnitStats unitStast;

    public UnitAction(GameObject enemy,GameObject go)
    {
        this.targetEnemy = enemy;
        this.agent = go.GetComponent<NavMeshAgent>();
        this.unitStast = go.GetComponent<UnitStats>();
    }

    public UnitAction(Vector3 targetPos, GameObject go)
    {
        this.targetEnemy = null;
        this.targetPosition = targetPos;
        this.agent = go.GetComponent<NavMeshAgent>();
        this.unitStast = go.GetComponent<UnitStats>(); 
    }


    // Start is called before the first frame update
    abstract public void Start();

    // Update is called once per frame
    abstract public void Update();
}
