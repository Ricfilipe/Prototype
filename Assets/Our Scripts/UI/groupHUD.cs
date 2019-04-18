using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class groupHUD : MonoBehaviour
{

    public GameObject[] buttons = new GameObject[10];
    public GameObject gameManger;
    private GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject but in buttons) {
            but.active = false;
        }

        this.gm = gameManger.GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i<gm.groups.Length; i++)
        {
            if (gm.groups[i] != null || gm.groups[i].Count != 0)
            {
                buttons[i].active = true;

            }
            else {
                buttons[i].active = false;
            }

        }
       
        
    }
}
