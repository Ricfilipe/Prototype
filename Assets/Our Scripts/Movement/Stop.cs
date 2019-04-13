using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stop : UnitAction
{
    public Stop(GameObject enemy, GameObject go) : base(enemy, go)
    {
    }

    public Stop(Vector3 targetPos, GameObject go) : base(targetPos, go)
    {
    }

    public override void Start()
    {
        throw new System.NotImplementedException();
    }

    public override void Update()
    {
        throw new System.NotImplementedException();
    }

}
