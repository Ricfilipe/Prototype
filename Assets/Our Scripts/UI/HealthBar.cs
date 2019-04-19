using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public GameObject unit;
    private UnitStats myUnitStats;
    // Start is called before the first frame update
    void Start()
    {
        this.myUnitStats = unit.GetComponent<UnitStats>();
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponent<RectTransform>().localScale = new Vector3(this.myUnitStats.HP/(this.myUnitStats.getMaxHP()+0.0f),1,1);
        Vector3 pos = unit.transform.position;
         gameObject.transform.parent.parent.position= new Vector3(pos.x, pos.y + 3  , pos.z);
     


    }
}


