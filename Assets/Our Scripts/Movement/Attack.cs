using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : UnitAction
{
    public Attack(GameObject enemy, GameObject go, GameManager gm) : base(enemy, go, gm)
    {
    }

    public Attack(Vector3 targetPos, GameObject go,GameManager gm) : base(targetPos, go,gm)
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
