using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : UnitAction
{
    public Move(GameObject enemy, GameObject go, GameManager gm) : base(enemy, go, gm)
    {
    }

    public Move(Vector3 targetPos, GameObject go, GameManager gm) : base(targetPos, go, gm)
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
