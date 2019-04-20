using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class generateSelection : MonoBehaviour
{

    public GameObject gameManager;
    public GameObject prefab;
    public GameObject up, down,text;
    public List<Sprite> images;

    [HideInInspector]
    public bool change = false;
    [HideInInspector]
    public int seq ;

    private Text realText;
    private List<GameObject> selected;
    private List<GameObject> selectionMenu;

    // Start is called before the first frame update
    void Start()
    {
        this.realText = text.GetComponent<UnityEngine.UI.Text>();
        seq = 0;
        this.selectionMenu = new List<GameObject>();
        this.selected = new List<GameObject>();

        down.active = false;
        up.active = false;
        realText.text = "";
    }

    // Update is called once per frame
    void Update()
    { 
       
        if (change)
        {
            selected.Clear();
            GameManager gm = gameManager.GetComponent<GameManager>();
            if (gm.King != null)
            {
                this.selected.Add(gm.King);
            }
            this.selected.AddRange(gm.archerSelected);
            this.selected.AddRange(gm.knightSelected);
            foreach (GameObject go in selectionMenu)
                Destroy(go);
            selectionMenu.Clear();
            change = false;
            for (int i = seq*10;i<this.selected.Count;i++)
            {
                int j = i % 10;
                GameObject ui = Instantiate(prefab, transform);
               ui.GetComponent<RectTransform>().anchoredPosition = new Vector2(59 + 83 * (j % 5), -52 - 84 * (j / 5));
                ui.GetComponent<PanelInterface>().selection = this.gameObject;

                switch (selected[i].GetComponent<UnitStats>().troop)
                {
                    case UnitStats.Troops.King:
                        ui.GetComponent<PanelInterface>().putImage(images[0]);
                        ui.GetComponent<PanelInterface>().progressionActive.active = false;
                        ui.GetComponent<PanelInterface>().progressionCD.active = true;
                        break;
                    case UnitStats.Troops.Archer:
                        ui.GetComponent<PanelInterface>().putImage(images[1]);
                        ui.GetComponent<PanelInterface>().progressionActive.active = false;
                        ui.GetComponent<PanelInterface>().progressionCD.active = true;
                        break;
                    case UnitStats.Troops.Infantry:
                        ui.GetComponent<PanelInterface>().putImage(images[2]);
                        ui.GetComponent<PanelInterface>().progressionActive.active = false;
                        ui.GetComponent<PanelInterface>().progressionCD.active = false;
                        break;

                }
               

                selectionMenu.Add(ui);
               
                if (i%10 == 9)
                {
                    break;
                }
            }
            if (seq>0) {
                up.active = true;
            }
            else
            {
                up.active = false;
            }

            if (seq < selected.Count/10)
            {
                down.active = true;
            }
            else
            {
                down.active = false;
            }

            if (selected.Count / 10 > 0)
            {
                realText.text = (seq+1)+"/"+ (1+selected.Count / 10);
            }
            else {
                realText.text = "";
            }

        }
        updateHP();
        for (int i=0; i<selected.Count;i++)
        {
            selectionMenu[i].GetComponent<PanelInterface>().UpdateProgressionBar(selected[i].GetComponent<MovementManager>());
        }
    }

    private void updateHP()
    {
        for(int i=0; i < selectionMenu.Count; i++)
        {
            selectionMenu[i].GetComponent<PanelInterface>().updateHPbar(selected[i]);
        }
    }


    public void incSeq()
    {
        seq++;
        change = true;
    }

    public void decSeq()
    {
        seq--;
        change = true;
    }

    public void changeSelection(GameObject panel) {
        int idx = this.selectionMenu.FindIndex(0, selectionMenu.Count, p => p == panel);
        GameObject go = selected[idx];
        if (Input.GetKey(KeyCode.LeftShift)){
            gameManager.GetComponent<GameManager>().removeFromSelection(go);
        }
        else {
            gameManager.GetComponent<GameManager>().clearSelection();
            gameManager.GetComponent<GameManager>().addToSelection(go);
        }
    }
}
