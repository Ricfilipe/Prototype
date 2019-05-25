using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Formation 
{
    private GameObject go;

    public Formation(GameObject go)
    {
        this.go = go;
    }

    public bool ofensive;
    public void doFormation(List<GameObject> army, GameObject leader)
    {
        if (army.Count == 0)
        {
            return;
        }

        doOffensiveFormation(army,leader);
        
    }

    protected abstract void doOffensiveFormation(List<GameObject> army, GameObject leader);

    protected abstract void doDefensiveFormation(List<GameObject> army, GameObject leader);
}
