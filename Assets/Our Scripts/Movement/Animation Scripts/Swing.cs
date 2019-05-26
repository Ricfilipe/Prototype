using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swing : MonoBehaviour
{
    public AudioSource[] SwingSword;
    
    public void swing()
    {
        SwingSword[Random.Range(0, SwingSword.Length)].Play();
        int damage = GetComponentInParent<UnitStats>().getAD();
        UnitStats enemyStats = null;
        if (GetComponentInParent<UnitStats>().team == UnitStats.Team.England) {
             enemyStats = GetComponentInParent<MovementManager>().target.GetComponentInParent<UnitStats>();
            if (enemyStats.HP>0 && enemyStats.HP - damage <= 0)
            {
                GameObject.FindGameObjectWithTag("GameManager").GetComponent<Metrics>().countKnightKill();
            }
        }
        else
        {
             enemyStats = GetComponentInParent<Enemies>().nearestUnit.GetComponentInParent<UnitStats>();
        }
        if (!enemyStats.undead)
            enemyStats.HP-=damage;
    }
}
