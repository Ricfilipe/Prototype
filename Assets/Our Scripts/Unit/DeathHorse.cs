using MalbersAnimations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DeathHorse : MonoBehaviour
{

    public enum State
    {
        Alive,
        Dying
    }

    public GameObject Icon;
    public GameObject HP;
    [HideInInspector]
    public State state = State.Alive;
    public AudioSource SoundOfDying;

    public IEnumerator Dead()
    {
        if (state == State.Alive)
        {
            state = State.Dying;
            
            GetComponentInChildren<NavMeshAgent>().enabled = false;
            GetComponentInParent<UnitStats>().dead = true;
            SoundOfDying.Play();
            GetComponent<Animal>().Death = true;
            HP.active = false;
            Icon.active = false;
            yield return new WaitForSeconds(3f);
            Destroy(transform.parent.gameObject);
        }
    }
}
