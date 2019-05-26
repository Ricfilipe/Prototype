using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeItRain: AbilityTimer
{

    private static float attackRatio = 0.5f;
    public AudioSource[] archerSound;
    protected override void apply()
    {
        GetComponent<UnitStats>().attackSpeed = GetComponent<UnitStats>().attackSpeed * attackRatio;
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<Metrics>().countAbilityArcher();
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
