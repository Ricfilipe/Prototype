using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Dying : MonoBehaviour
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

    public  IEnumerator Dead()
    {
        if (state == State.Alive) 
        {
            //Actual Cancer
            Vector3 vector = transform.position;
            transform.parent.parent.position = new Vector3(vector.x, 0f, vector.z);
            transform.localPosition = new Vector3(0, 0, 0);
            Quaternion qua = transform.rotation;
            transform.parent.parent.rotation = qua;
            transform.rotation = new Quaternion(0f, 0f, 0f,0f);
            state = State.Dying;
            GetComponentInParent<Animator>().Play("death");
            GetComponentInParent<Collider>().enabled = false ;
            GetComponent<NavMeshAgent>().enabled = false;
            GetComponent<UnitStats>().dead = true;
            
            HP.active = false;
            Icon.active = false;
            yield return new WaitForSeconds(3f);
            Destroy(transform.parent.parent.gameObject);
        }
    }
}
