using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityTimer : Ability
{




    public override IEnumerator DoAbility()
    {
        if(state == State.Available)
        {
            state = State.Active;            
             apply();
            timeOnActivation = Time.time;
             yield return new WaitForSeconds(activeTime);
           
            timeOnCooldown = Time.time;
            state = State.OnCooldown;
            whileOnCooldown();
            yield return new WaitForSeconds(cooldown);
          
            state = State.Available;
            whileAvailable();
        }
    }

    protected abstract void apply();

    protected abstract void whileOnCooldown();

    protected abstract void whileAvailable();



}
