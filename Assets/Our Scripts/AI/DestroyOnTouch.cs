using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnTouch : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Base")
        {
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().baseDestroyed = true;
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().baseSelected = false;
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().enemyPool.Clear();
            Destroy(transform.parent.parent.parent.gameObject);
        }
    }


}
