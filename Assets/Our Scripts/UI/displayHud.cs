using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class displayHud : MonoBehaviour
{
    public GameObject textObject, imageObject, descriptionObject,gameManager;
    private Sprite[] images = new Sprite[3];

    private Text name, description;
    private GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        this.name = textObject.GetComponent<Text>();
        this.description = descriptionObject.GetComponent<Text>();
        this.gm = gameManager.GetComponent<GameManager>();
        images[0] = Resources.Load<Sprite>("Rei");
        images[1] = Resources.Load<Sprite>("Infantaria2");
        images[2] = Resources.Load<Sprite>("Arqueiro");
        imageObject.GetComponent<Image>().sprite = images[0];
    }

    // Update is called once per frame
    void Update()
    {
        this.name.text = "";
        this.description.text = "";
        if (gm.King != null && gm.archerSelected.Count == 0 && gm.knightSelected.Count == 0)
        {
            this.name.text = "King Henry V";
            writeDescriptionSing(gm.King.GetComponentInParent<UnitStats>());
            imageObject.GetComponent<Image>().sprite = images[0];
        }
        else if (gm.King == null && gm.archerSelected.Count == 1 && gm.knightSelected.Count == 0)
        {
            this.name.text = "Archer";
            writeDescriptionSing(gm.archerSelected[0].GetComponent<UnitStats>());
            imageObject.GetComponent<Image>().sprite = images[2];
        }
        else if (gm.King == null && gm.archerSelected.Count == 0 && gm.knightSelected.Count == 1)
        { 
            this.name.text = "Men-At-Arms";
            writeDescriptionSing(gm.knightSelected[0].GetComponent<UnitStats>());
            imageObject.GetComponent<Image>().sprite = images[1];
        }
        else {
            if (gm.archerSelected.Count >= 1)
            {
                this.description.text = this.description.text + "Archer: " + gm.archerSelected.Count + "\n";
                imageObject.GetComponent<Image>().sprite = images[2];
            }
            if (gm.knightSelected.Count >= 1)
            {
                this.description.text = this.description.text + "Men-At-Arms: " + gm.knightSelected.Count + "\n";
                imageObject.GetComponent<Image>().sprite = images[1];
            }

            if (gm.King != null)
            {
                this.description.text = this.description.text + "King: 1\n";
                imageObject.GetComponent<Image>().sprite = images[0];
            }


        }
    }
    private void writeDescriptionSing(UnitStats stats)
        {
            this.description.text = "HP: " + stats.HP + "/"+ stats.getMaxHP()+"\n" +
                           "Attack: "+stats.getAD()+"\n" +
                           "Speed: "+stats.getSpeed()+"\n";
        }
    
}
