using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForTheKing : AbilityTimer
{
    private List<UnitStats> unitsChanged = new List<UnitStats>();
    private float adRatio = 1.5f;



    protected override void apply()
    {
        
    }

    protected override void whileAvailable()
    {
       
    }

    protected override void whileOnCooldown()
    {
        foreach (UnitStats stat in unitsChanged)
        {
            stat.adMultiplier = 1f;
        }         
    }

}
