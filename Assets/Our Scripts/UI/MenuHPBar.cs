using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuHPBar:MonoBehaviour
{
    [HideInInspector]
    public GameObject unit;
    
    public void updateBar(GameObject unit)
    {
        UnitStats stats = unit.GetComponent<UnitStats>();
        gameObject.GetComponent<RectTransform>().localScale = new Vector3(stats.HP / (stats.getMaxHP()+0f), 1, 1);
       
   



    }
}
