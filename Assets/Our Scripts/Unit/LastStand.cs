using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastStand : AbilityOneTime
{
    protected override void afterUse()
    {
        Debug.Log("test");
        stats.undead = false;
        stats.adMultiplier = 1f;
        stats.HP = 0; 
    }

    protected override void apply()
    {
        stats.undead = true;
        stats.adMultiplier = 2f;
        stats.HP = 1;
    }
}
