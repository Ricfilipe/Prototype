using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastStand : AbilityOneTime
{

    public AudioSource[] sounds;

    protected override void afterUse()
    {
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
