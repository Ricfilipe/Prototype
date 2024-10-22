﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability:MonoBehaviour
{
    public int activeTime, cooldown;
    public String name;
    public bool hasActivation,hasCd;
    [HideInInspector]
    public float timeOnActivation, timeOnCooldown;
    protected UnitStats stats;

    private void Start()
    {
        this.stats = GetComponent<UnitStats>();
    }

    public enum State
    {
        OnCooldown,
        Using,
        Available,
        Active
    }

    public State state = State.Available;

     public abstract IEnumerator DoAbility();

}
