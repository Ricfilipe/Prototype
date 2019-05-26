using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrowFollow : MonoBehaviour
{

    [HideInInspector]
    public bool follow = false;

    private GameObject target;
    private int frame = 0;
    private Vector3 initialPos;
    private Vector3 targetPos = new Vector3(0, 0, 0);
    public int damage;
    public AudioSource[] hit;

    // Update is called once per frame
    void Update()
    {


        if (follow)
        {

            if (frame == 15)
            {
                
                hit[Random.Range(0,hit.Length)].Play();
                frame = 0;
                if (target != null)
                {
                    if(!target.GetComponentInParent<UnitStats>().undead)
                     if(target.GetComponentInParent<UnitStats>().team == UnitStats.Team.France)
                        {
                            if (target.GetComponentInParent<UnitStats>().HP>0 && target.GetComponentInParent<UnitStats>().HP - damage <= 0)
                            {
                                GameObject.FindGameObjectWithTag("GameManager").GetComponent<Metrics>().countArcherKill();
                            }

                        }
                    target.GetComponentInParent<UnitStats>().HP -= damage;
                }
                Destroy(gameObject);
            }
            if (frame==0)
            initialPos = transform.position;

            
            if (target != null)
            {
                targetPos = target.transform.position;
            }

            frame++;
            transform.position = (targetPos-initialPos) / (15f/frame) + initialPos;
            
        }
    }

    public void markTarget(GameObject target)
    {
        this.target = target;
        follow = true;
    }
}
