﻿using System.Collections;
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
            writeDescriptionSing(gm.King.GetComponent<UnitStats>());
        }
        else if (gm.King == null && gm.archerSelected.Count == 1 && gm.knightSelected.Count == 0)
        {
            this.name.text = "Men-At-Arms";
            writeDescriptionSing(gm.knightSelected[0].GetComponent<UnitStats>());
        }
        else if (gm.King == null && gm.archerSelected.Count == 0 && gm.knightSelected.Count == 1)
        { 
            this.name.text = "Archer";
            writeDescriptionSing(gm.archerSelected[0].GetComponent<UnitStats>());
        }
        else {

        }
    }
    private void writeDescriptionSing(UnitStats stats)
        {
            this.description.text = "HP: " + stats.HP + "/"+ stats.getMaxHP()+"\n" +
                           "Attack: "+stats.getAD()+"\n" +
                           "Speed: "+stats.getSpeed()+"\n";
        }
    
}
