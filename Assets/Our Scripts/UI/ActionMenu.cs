using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionMenu : MonoBehaviour
{

    public GameObject gameManager;
    public GameObject[] actions = new GameObject[9];

    private Button[] actionButtons = new Button[9];
    private GameManager gm;
    private bool upgradeMenu;

    public GameObject archer;

    // Start is called before the first frame update
    void Start()
    {
        this.gm = gameManager.GetComponent<GameManager>();
        for (int i = 0; i<actions.Length;i++) {
            actionButtons[i] = actions[i].GetComponent<Button>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        emptyButtons();
        if (gm.baseSelected)
        {
            if (upgradeMenu)
            {
                //Menu para os Updgrades
                actions[0].GetComponentInChildren<Text>().text = "Knight HP\n$150";
                actions[1].GetComponentInChildren<Text>().text = "Knight Attack\n$250";
                actions[2].GetComponentInChildren<Text>().text = "Knight Speed\n$200";

                actions[3].GetComponentInChildren<Text>().text = "Archer Arnmor\n$200";
                actions[4].GetComponentInChildren<Text>().text = "Archer Attack\n$150";
                actions[5].GetComponentInChildren<Text>().text = "Archer Speed\n$100";


                //TODO add Listeners
                actionButtons[0].interactable = true;
                actionButtons[1].interactable = true;
                actionButtons[2].interactable = true;
                actionButtons[3].interactable = true;
                actionButtons[4].interactable = true;
                actionButtons[5].interactable = true;


                actionButtons[6].interactable = true;
                actionButtons[6].onClick.AddListener(backMenu);
                actions[6].GetComponentInChildren<Text>().text = "<-";
            }
            else
            {
                //Menu para Base
                actions[1].GetComponentInChildren<Text>().text = "Archer\n$200";
                actions[0].GetComponentInChildren<Text>().text = "Knight\n$150";
                if (gm.silver > 150) //valor provisório
                {
                    actionButtons[0].interactable = true;
                    actionButtons[0].onClick.AddListener(buyKnight);

                }
                else
                {
                    actionButtons[0].interactable = false;
                }

                if (gm.silver > 200) //valor provisório
                {
                    actionButtons[1].interactable = true;
                    actionButtons[1].onClick.AddListener(buyArcher);

                }
                else
                {
                    actionButtons[1].interactable = false;
                }

                actionButtons[2].interactable = true;
                actionButtons[2].onClick.AddListener(upgrade);
                actions[2].GetComponentInChildren<Text>().text = "Upgrades";
            }


        }
        else if (gm.archerSelected.Count > 0 || gm.knightSelected.Count > 0 || gm.King != null)
        {
            upgradeMenu = false;
            actions[0].GetComponentInChildren<Text>().text = "Attack";
            actions[1].GetComponentInChildren<Text>().text = "Stop";
            actions[2].GetComponentInChildren<Text>().text = "Move";
            actionButtons[0].interactable = true;
            actionButtons[0].onClick.AddListener(attackMove);
            actionButtons[1].interactable = true;
            actionButtons[1].onClick.AddListener(stopMove);
            actionButtons[2].interactable = true;
            actionButtons[2].onClick.AddListener(moveMove);



            if (gm.archerSelected.Count > 0)
            {
                actions[3].GetComponentInChildren<Text>().text = "Make it Rain";
                actionButtons[3].interactable = true;
                actionButtons[3].onClick.AddListener(archerAbility);
            }



            if (gm.King != null)
            {
                actions[5].GetComponentInChildren<Text>().text = "For the King";
                actionButtons[5].interactable = true;
                actionButtons[5].onClick.AddListener(archerAbility);
            }



            actionButtons[7].interactable = true;
            actionButtons[7].onClick.AddListener(selectBase);
            actions[7].GetComponentInChildren<Text>().text = "BASE";
            

        }
        else {
            actionButtons[7].interactable = true;
            actionButtons[7].onClick.AddListener(selectBase);
            actions[7].GetComponentInChildren<Text>().text = "BASE";
        }

        
        
    }

    public void attackMove()
    {
        //TODO
    }

    public void moveMove()
    {
        //TODO
    }

    public void stopMove()
    {
        //TODO
    }

    public void knightAbility()
    {
        //TODO
    }


    public void archerAbility()
    {
        //TODO
    }

    public void kingAbility()
    {
        //TODO
    }

    public void buyKnight() {
        //TODO
    }


    public void buyArcher()
    {
       GameObject go = Instantiate(archer);
        go.GetComponent<MovementManager>().gm = this.gameManager;
    }



    public void upgrade()
    {
        upgradeMenu = true;
    }

    public void backMenu()
    {
        upgradeMenu = false;
    }

    public void selectBase()
    {
        gm.baseSelected = true;
        gm.clearSelection();
    }



    void emptyButtons() {

        foreach (GameObject bt in actions)
        {
            bt.GetComponent<Button>().interactable = false;
            bt.GetComponent<Button>().onClick.RemoveAllListeners();
            bt.GetComponentInChildren<Text>().text = "";
        }
    }





}
