using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeItRain: AbilityTimer
{

     private static float attackRatio = 0.5f;

    protected override void apply()
    {
        GetComponent<UnitStats>().attackSpeed = GetComponent<UnitStats>().attackSpeed * attackRatio;
        GetComponent<MovementManager>().enchanced = true;
    }

    protected override void whileAvailable()
    {
       
     
    }

    protected override void whileOnCooldown()
    {
        GetComponent<UnitStats>().attackSpeed = GetComponent<UnitStats>().attackSpeed / attackRatio;
        GetComponent<MovementManager>().enchanced = false;
    }


}
