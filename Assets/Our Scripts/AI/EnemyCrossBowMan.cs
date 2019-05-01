using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCrossBowMan : Enemies
{

    protected override void Start()
    {
        base.Start();
        TotalHealth = unitStats.MaxHP;
        currentHealth = TotalHealth;
        movementSpeedRatio = 2;
        detectUnitsRadius = 90.0f;
    }

    protected override void EnemyMovement()
    {
        base.EnemyMovement();
    }

    //Enemy attack style into code
    protected override void Attack()
    {
        base.Attack();
    }

    protected override void DetectTargetingUnits()
    {
        center = new Vector3(transform.position.x, 0, transform.position.z);
        base.DetectTargetingUnits();
    }

}
