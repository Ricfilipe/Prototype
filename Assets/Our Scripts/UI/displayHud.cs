using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class displayHud : MonoBehaviour
{
    public GameObject textObject, imageObject, descriptionObject,gameManager;


    private Text name, description;
    private GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        this.name = textObject.GetComponent<Text>();
        this.description = descriptionObject.GetComponent<Text>();
        this.gm = gameManager.GetComponent<GameManager>();
           
    }

    // Update is called once per frame
    void Update()
    {
        if (gm.King != null && gm.archerSelected.Count == 0 && gm.knightSelected.Count == 0)
        {
            this.name.text = "King Henry V";
            this.description.text =

        }
        else if (gm.King == null && gm.archerSelected.Count == 1 && gm.knightSelected.Count == 0)
        {


        }
        else if (gm.King == null && gm.archerSelected.Count == 0 && gm.knightSelected.Count == 1)
        {


        }
        else {

        }


    }
}
