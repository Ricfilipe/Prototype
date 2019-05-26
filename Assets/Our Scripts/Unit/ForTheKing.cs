using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForTheKing : AbilityTimer
{
    private List<UnitStats> unitsChanged = new List<UnitStats>();
    private float adRatio = 1.5f;
    public AudioSource[] kingSound;
    public float detectUnitsRadius;

    protected override void apply()
    {
        kingSound[Random.Range(0, kingSound.Length)].Play();
        List<Collider> unitsCollided = new List<Collider>();
        List<Collider> currentUnitsInRange = new List<Collider>();
        unitsCollided.AddRange(Physics.OverlapSphere(gameObject.transform.position, detectUnitsRadius));
        foreach (Collider c in unitsCollided.FindAll(x => x.CompareTag("MyUnit")))
        {
            UnitStats stat = c.gameObject.GetComponentInParent<UnitStats>();
            unitsChanged.Add(stat);
            stat.adMultiplier=1.5f;
        }
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
