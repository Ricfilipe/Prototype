using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shootArrow : MonoBehaviour
{
    public GameObject arrow;
    public void shoot(GameObject target)
    {
        GameObject arrow = Instantiate(this.arrow,null);
        arrow.transform.position = transform.position;
        arrow.transform.rotation = transform.rotation;
        arrow.GetComponent<arrowFollow>().markTarget(target);
        arrow.GetComponent<arrowFollow>().damage = GetComponentInParent<UnitStats>().getAD();
    }
}
