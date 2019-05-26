using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossbowFormation : Formation
{
    private int colDef = 6;
    public CrossbowFormation(GameObject go) : base(go)
    {
    }

    protected override void doDefensiveFormation(List<GameObject> army, GameObject leader)
    {


    }

    protected override void doOffensiveFormation(List<GameObject> army, GameObject leader)
    {
        var numlin = army.Count / colDef;
        float x = 1.5f;
        int i = 0;
        while (i < numlin)
        {
            var start = i * colDef;
            army[start + 2].GetComponentInChildren<Enemies>().offset = new Vector2(7.5f, -x * i - 1.5f);
            army[start + 1].GetComponentInChildren<Enemies>().offset = new Vector2(4.5f, -x * i - 1.5f);
            army[start + 0].GetComponentInChildren<Enemies>().offset = new Vector2(1.5f, -x * i - 1.5f);
            army[start + 3].GetComponentInChildren<Enemies>().offset = new Vector2(-1.5f, -x * i - 1.5f);
            army[start + 4].GetComponentInChildren<Enemies>().offset = new Vector2(-4.5f, -x * i - 1.5f);
            army[start + 5].GetComponentInChildren<Enemies>().offset = new Vector2(-7.5f, -x * i - 1.5f);
            i++;

        }
        var numLast = army.Count - i * colDef;
        float y = 1.5f;
        for (int j = i * colDef; j < army.Count; j++)
        {


            if (numLast % 2 == 0)
            {
                if (j % 2 == 0)
                {
                    army[j].GetComponentInChildren<Enemies>().offset = new Vector2(y, -x * i - 1.5f);
                }
                else
                {
                    army[j].GetComponentInChildren<Enemies>().offset = new Vector2(-y, -x * i - 1.5f);
                    y += 3f;
                }
            }
            else
            {
                if (j - i * colDef == 0)
                {
                    army[j].GetComponentInChildren<Enemies>().offset = new Vector2(0, -x * i - 1.5f);
                    y = y * 2;
                }
                else if (j % 2 == 0)
                {
                    army[j].GetComponentInChildren<Enemies>().offset = new Vector2(y, -x * i - 1.5f);
                    y += 3f;
                }
                else
                {
                    army[j].GetComponentInChildren<Enemies>().offset = new Vector2(-y, -x * i - 1.5f);

                }

            }
        }
    }


}
