using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : UnitAction
{
    public Move(GameObject enemy, GameObject go) : base(enemy, go)
    {
    }

    public Move(Vector3 targetPos, GameObject go) : base(targetPos, go)
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
