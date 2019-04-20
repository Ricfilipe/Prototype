using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityOneTime : Ability
{
    public override IEnumerator DoAbility()
    {
        if (state == State.Available)
        {
            state = State.Active;
            apply();
            timeOnActivation = Time.time;
            
            yield return new WaitForSeconds(activeTime);
            afterUse();
            state = State.OnCooldown;

        }
        
    }

    protected abstract void apply();
    protected abstract void afterUse();
}
