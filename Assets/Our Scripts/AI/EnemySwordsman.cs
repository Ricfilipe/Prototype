using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySwordsman : Enemies
{

    protected override void Start()
    {
        movementSpeedRatio = 2;
        base.Start();
    }

    //Enemy attack style into code
    protected override void Attack()
    {

    }

}
