using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shootArrow : MonoBehaviour
{
    public GameObject arrow;
    public GameObject currentArrow;
    public AudioSource[] ShootArrow;
    public void shoot()
    {
        ShootArrow[Random.Range(0, ShootArrow.Length)].Play();
        GameObject arrow = Instantiate(this.arrow,null);
        arrow.transform.position = currentArrow.transform.position;
        arrow.transform.rotation = currentArrow.transform.rotation;
        if (GetComponentInParent<UnitStats>().team == UnitStats.Team.England) {
            arrow.GetComponent<arrowFollow>().markTarget(GetComponentInParent<MovementManager>().target);
        }
        else
        {
            arrow.GetComponent<arrowFollow>().markTarget(GetComponentInParent<Enemies>().nearestUnit);
        }
        arrow.GetComponent<arrowFollow>().damage = GetComponentInParent<UnitStats>().getAD();
    }
}
