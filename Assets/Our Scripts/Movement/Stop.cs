﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Stop : UnitAction
{
    public Stop(GameObject enemy, GameObject go, GameManager gm) : base(enemy, go, gm)
    {
    }

    public Stop(Vector3 targetPos, GameObject go, GameManager gm) : base(targetPos, go, gm)
    {
    }

    public override void Start()
    {
        agent.GetComponentInParent<MovementManager>().target = null;
        agent.GetComponentInChildren<NavMeshAgent>().isStopped = true;
    }

    public override void Update()
    {
       
    }

}
