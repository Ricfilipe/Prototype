﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class UnitAction 
{
    protected Vector3 targetPosition;
    protected GameObject targetEnemy;
    protected NavMeshAgent agent;
    protected UnitStats unitStast;
    protected GameManager gm;

    public UnitAction(GameObject enemy,GameObject go,GameManager gm)
    {
        this.targetEnemy = enemy;
        this.agent = go.GetComponent<NavMeshAgent>();
        this.unitStast = go.GetComponent<UnitStats>();
        this.gm = gm;
    }

    public UnitAction(Vector3 targetPos, GameObject go, GameManager gm)
    {
        this.targetEnemy = null;
        this.targetPosition = targetPos;
        this.agent = go.GetComponent<NavMeshAgent>();
        this.unitStast = go.GetComponent<UnitStats>();
        this.gm = gm;
    }


    // Start is called before the first frame update
    abstract public void Start();

    // Update is called once per frame
    abstract public void Update();
}
