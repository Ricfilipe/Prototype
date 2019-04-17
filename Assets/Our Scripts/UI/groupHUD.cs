using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class groupHUD : MonoBehaviour
{

    public GameObject[] buttons = new GameObject[10];

    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject but in buttons) {
            but.active = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
