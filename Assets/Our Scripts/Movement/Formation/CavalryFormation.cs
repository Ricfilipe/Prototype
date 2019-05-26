using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CavalryFormation : Formation
{
    public CavalryFormation(GameObject go) : base(go)
    {
    }

    protected override void doDefensiveFormation(List<GameObject> army, GameObject leader)
    {
        throw new System.NotImplementedException();
    }

    protected override void doOffensiveFormation(List<GameObject> army, GameObject leader)
    {
        Vector2 localOffset = new Vector2(1f, 3f);
        if (army.Count % 2 == 0)
        {
            localOffset = new Vector2(1.5f, localOffset.y / 2f);
            for (int i = 0; i < army.Count; i++)
            {
                army[i].GetComponentInChildren<NavMeshAgent>().enabled = true;
                Enemies follow = army[i].GetComponentInChildren<Enemies>();
                follow.leader = leader;
                if (i % 2 == 0)
                {
                    follow.offset = localOffset;
                }
                else if (i % 2 - 1 == 0)
                {
                    follow.offset = new Vector2(localOffset.x, -localOffset.y);
                    localOffset = new Vector2(localOffset.x + 1.5f, localOffset.y + 1.5f);
                }

            }
        }
        else {
            localOffset = new Vector2(3f, 3f);
            for (int i = 0; i < army.Count; i++)
            {

                Enemies follow = army[i].GetComponentInChildren<Enemies>();
                follow.leader = leader;
                if (i == 0)
                {
                    follow.offset = new Vector2(0, 0);
                }
                else if (i % 2 - 1 == 0)
                {
                    follow.offset = localOffset;
                }
                else
                {
                    follow.offset = new Vector2(localOffset.x, -localOffset.y);
                    localOffset = new Vector2(localOffset.x * 3f, localOffset.y * 3f);
                }
            }
        }
    }

}
