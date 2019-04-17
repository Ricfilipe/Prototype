using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArcher : Enemies
{

    protected override void Start()
    {
        movementSpeedRatio = 2;
        base.Start();
    }

    protected override void EnemyMovement()
    {
        base.EnemyMovement();
    }

}
