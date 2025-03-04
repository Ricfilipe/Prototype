﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class ActionMenu : MonoBehaviour
{

    public GameObject gameManager;
    public GameObject[] actions = new GameObject[9];

    private Button[] actionButtons = new Button[9];
    private GameManager gm;
    private bool upgradeMenu;

    public GameObject archer;
    public GameObject knight;

    //static upgrade Knight costs
    private static int costKnightHPUpgrade = 150;
    private static int costKnightADUpgrade = 250;
    private static int costKnightSpeedUpgrade = 200;
    //static upgrade Archer costs
    private static int costArcherHPUpgrade = 200;
    private static int costArcherADUpgrade = 150;
    private static int costArcherSpeedUpgrade = 100;

    
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
        if (gm.baseDestroyed)
        {
            upgradeMenu = false;
        }
        if (gm.baseSelected)
        {
            if (upgradeMenu)
            {
                //Menu para os Updgrades
                if(UnitStats.knightLevel[0]<3)
                actions[0].GetComponentInChildren<Text>().text = "Knight HP\n"+(costKnightHPUpgrade + UnitStats.knightLevel[0]*100);
                if (UnitStats.knightLevel[1] < 3)
                    actions[1].GetComponentInChildren<Text>().text = "Knight Attack\n"+(costKnightADUpgrade + UnitStats.knightLevel[1]*100);
                if (UnitStats.knightLevel[2] < 3)
                    actions[2].GetComponentInChildren<Text>().text = "Knight Speed\n"+(costKnightSpeedUpgrade + UnitStats.knightLevel[2]*100);
                if (UnitStats.archerLevel[0] < 3)
                    actions[3].GetComponentInChildren<Text>().text = "Archer HP\n"+(costArcherHPUpgrade + UnitStats.archerLevel[0]*100);
                if (UnitStats.archerLevel[1] < 3)
                    actions[4].GetComponentInChildren<Text>().text = "Archer Attack\n"+(costArcherADUpgrade + UnitStats.archerLevel[1]*100);
                if (UnitStats.archerLevel[2] < 3)
                    actions[5].GetComponentInChildren<Text>().text = "Archer Speed\n"+(costArcherSpeedUpgrade + UnitStats.archerLevel[2]*100);

                if(gm.silver>= costKnightHPUpgrade + UnitStats.knightLevel[0] * 100 && UnitStats.knightLevel[0] < 3)
                {
                    actionButtons[0].interactable = true;
                    actionButtons[0].onClick.AddListener(knightUpgradeHP);
                }

                if(gm.silver >= costKnightADUpgrade + UnitStats.knightLevel[1] * 100 && UnitStats.knightLevel[1] < 3)
                {
                    actionButtons[1].interactable = true;
                    actionButtons[1].onClick.AddListener(knightUpgradeAttack);       
                }

                if (gm.silver >= costKnightSpeedUpgrade + UnitStats.knightLevel[2] * 100 && UnitStats.knightLevel[2] < 3)
                {
                    actionButtons[2].interactable = true;
                    actionButtons[2].onClick.AddListener(knightUpgradeSpeed);
                }


                if(gm.silver >= costArcherHPUpgrade + UnitStats.archerLevel[0] * 100 && UnitStats.archerLevel[0] < 3)
                {
                    actionButtons[3].interactable = true;
                    actionButtons[3].onClick.AddListener(archerUpgradeHP);
                }

                if(gm.silver >= costArcherADUpgrade + UnitStats.archerLevel[1] * 100 && UnitStats.archerLevel[1] < 3)
                {
                    actionButtons[4].interactable = true;
                    actionButtons[4].onClick.AddListener(archerUpgradeAttack);
                }
                if (gm.silver >= costArcherSpeedUpgrade + UnitStats.archerLevel[2] * 100 && UnitStats.archerLevel[2] < 3)
                {
                    actionButtons[5].interactable = true;
                    actionButtons[5].onClick.AddListener(archerUpgradeSpeed);
                }

             
                actionButtons[6].interactable = true;
                actionButtons[6].onClick.AddListener(backMenu);
                actions[6].GetComponentInChildren<Text>().text = "<-";
                upgradeShortcut();
            }
            else
            {
                //Menu para Base
                actions[1].GetComponentInChildren<Text>().text = "Archer\n100";
                actions[0].GetComponentInChildren<Text>().text = "Knight\n50";
                if (gm.silver > 50)
                {
                    actionButtons[0].interactable = true;
                    actionButtons[0].onClick.AddListener(buyKnight);

                }
                else
                {
                    actionButtons[0].interactable = false;
                }

                if (gm.silver > 100) 
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

                actionButtons[7].interactable = true;
                actionButtons[7].onClick.AddListener(moveWayPoint);
                actions[7].GetComponentInChildren<Text>().text = "Waypoint";
                baseMenuShorcut();
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
            abilitiesShortcut();


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
                actionButtons[5].onClick.AddListener(kingAbility);
            }


            if (!gm.baseDestroyed)
            {
                actionButtons[7].interactable = true;
                actionButtons[7].onClick.AddListener(selectBase);
                actions[7].GetComponentInChildren<Text>().text = "BASE";
                baseShortcut();
            }


        }
        else if (!gm.baseDestroyed)
        {

            actionButtons[7].interactable = true;
            actionButtons[7].onClick.AddListener(selectBase);
            actions[7].GetComponentInChildren<Text>().text = "BASE";
            baseShortcut();
        }

        
        
    }

    public void attackMove()
    {
        gameManager.GetComponent<Metrics>().IncActions();
        gm.changeToAttack();
    }

    public void moveMove()
    {
        gameManager.GetComponent<Metrics>().IncActions();
        gm.changeToMove();
    }

    public void stopMove()
    {
        gameManager.GetComponent<Metrics>().IncActions();
        gm.makeAction(null, GameManager.typeAction.Stop);
    }



    public void archerAbility()
    {
        AudioSource sound = null;
        gameManager.GetComponent<Metrics>().IncActions();
        foreach (GameObject gameObj in gm.archerSelected)
        {
            MakeItRain mir = (MakeItRain) gameObj.GetComponent<MovementManager>().abs[0];
            if (mir.state == Ability.State.Available)
            {
                if(sound==null)
                {
                    sound = mir.archerSound[Random.Range(0, mir.archerSound.Length)];
                }
                sound.Play();
            }
            StartCoroutine(mir.DoAbility());
        }
    }


    public void kingAbility()
    {
        if(gm.King != null)
        {
            gameManager.GetComponent<Metrics>().IncActions();
            StartCoroutine(gm.King.GetComponent<MovementManager>().abs[0].DoAbility());
        }
    }


    public void knightUpgradeAttack()
    {
        
        if(UnitStats.knightLevel[1]!=3 && gm.silver>= costKnightADUpgrade + 100* UnitStats.knightLevel[1])
        {
            gameManager.GetComponent<Metrics>().IncActions();
            gm.silver -= costKnightADUpgrade + 100 * UnitStats.knightLevel[1];
            UnitStats.InfantryAD = UnitStats.InfantryAD + 1;
            UnitStats.knightLevel[1]++;
        }
        
    }

    public void knightUpgradeHP()
    {
        if (UnitStats.knightLevel[0] != 3 && gm.silver >= costKnightHPUpgrade + 100 * UnitStats.knightLevel[0])
        {
            gameManager.GetComponent<Metrics>().IncActions();
            gm.silver -= costKnightHPUpgrade + 100 * UnitStats.knightLevel[0];
            UnitStats.InfantryMaxHP = UnitStats.InfantryMaxHP + 2;
            foreach(GameObject go in gm.myCharacterPool)
            {
                UnitStats stat = go.GetComponent<UnitStats>();
                if (stat.troop == UnitStats.Troops.Infantry)
                    go.GetComponent<UnitStats>().HP = go.GetComponent<UnitStats>().HP + 2;
            }
            UnitStats.knightLevel[0]++;
        }
    }

    public void knightUpgradeSpeed()
    {
        if (UnitStats.knightLevel[2] != 3 && gm.silver >= costKnightSpeedUpgrade + 100 * UnitStats.knightLevel[2])
        {
            gameManager.GetComponent<Metrics>().IncActions();
            gm.silver -= costKnightSpeedUpgrade + 100 * UnitStats.knightLevel[2];
            UnitStats.InfantrySpeed = UnitStats.InfantrySpeed + 2;
            UnitStats.knightLevel[2]++;
        }
    }

    public void archerUpgradeAttack()
    {
        if (UnitStats.archerLevel[1] != 3 && gm.silver >= costArcherADUpgrade + 100 * UnitStats.archerLevel[1])
        {

            gameManager.GetComponent<Metrics>().IncActions();
            gm.silver -= costArcherADUpgrade + 100 * UnitStats.archerLevel[1];
            UnitStats.ArcherAD = UnitStats.ArcherAD + 1;
            UnitStats.archerLevel[1]++;
        }
    }

    public void archerUpgradeHP()
    {
        if (UnitStats.archerLevel[0] != 3 && gm.silver >= costArcherHPUpgrade + 100 * UnitStats.archerLevel[0])
        {
            gameManager.GetComponent<Metrics>().IncActions();
            gm.silver -= costArcherHPUpgrade + 100 * UnitStats.archerLevel[0];
            UnitStats.ArcherMaxHP = UnitStats.ArcherMaxHP + 2;
            foreach (GameObject go in gm.myCharacterPool)
            {
                UnitStats stat = go.GetComponent<UnitStats>();
                if (stat.troop == UnitStats.Troops.Archer)
                    go.GetComponent<UnitStats>().HP = go.GetComponent<UnitStats>().HP + 2;
            }
            UnitStats.archerLevel[0]++;
        }
    }

    public void archerUpgradeSpeed()
    {
        if (UnitStats.archerLevel[2] != 3 && gm.silver >= costArcherSpeedUpgrade + 100 * UnitStats.archerLevel[2])
        {
            gameManager.GetComponent<Metrics>().IncActions();
            gm.silver -= costArcherSpeedUpgrade + 100 * UnitStats.archerLevel[2];
            UnitStats.ArcherSpeed = UnitStats.ArcherSpeed + 2;
            UnitStats.archerLevel[2]++;
        }
    }




    public void buyKnight() {
        if (gm.silver >= 50)
        {
            gameManager.GetComponent<Metrics>().IncActions();
            gm.silver = gm.silver - 50;
            GameObject go = Instantiate(knight);
            spawn(go);
        }
    }


    public void buyArcher()
    {
        if (gm.silver >= 100)
        {
            gameManager.GetComponent<Metrics>().IncActions();
            gm.silver = gm.silver - 100;
            GameObject go = Instantiate(archer);
            spawn(go);
        }
    }



    public void upgrade()
    {
        gameManager.GetComponent<Metrics>().IncActions();
        upgradeMenu = true;
    }

    public void backMenu()
    {
        gameManager.GetComponent<Metrics>().IncActions();
        upgradeMenu = false;
    }

    public void selectBase()
    {
        if (!gm.baseDestroyed)
        {
            gameManager.GetComponent<Metrics>().IncActions();
            gm.baseSelected = true;
            gm.Base.GetComponent<Outline>().enabled = true;
            gm.banner.active = true;
            gm.clearSelection();
        }
    }



    void emptyButtons() {

        foreach (GameObject bt in actions)
        {
            bt.GetComponent<Button>().interactable = false;
            bt.GetComponent<Button>().onClick.RemoveAllListeners();
            bt.GetComponentInChildren<Text>().text = "";
        }
    }

    private void spawn(GameObject go)
    {
        go.transform.position=new Vector3(196.4982f, 0  , -2.590393f);
        go.GetComponentInChildren<NavMeshAgent>().destination = gm.banner.transform.position;
        go.GetComponentInChildren<NavMeshAgent>().isStopped = false ;
        go.GetComponentInChildren<MovementManager>().spaningSound[Random.Range(0, go.GetComponentInChildren<MovementManager>().spaningSound.Length)].Play();
    }

    private void moveWayPoint()
    {
        gameManager.GetComponent<Metrics>().IncActions();
        gm.currentAction = GameManager.typeAction.Banner;
    }

    private void baseShortcut()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            selectBase();
        }

    }

    private void abilitiesShortcut()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            archerAbility();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            kingAbility();
        }

    }

    private void baseMenuShorcut()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            buyArcher();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            buyKnight();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            upgrade();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            moveWayPoint();
        }

    }


    private void upgradeShortcut()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            knightUpgradeAttack();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            knightUpgradeHP();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            knightUpgradeSpeed();
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            archerUpgradeHP();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            archerUpgradeAttack();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            archerUpgradeSpeed();
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            backMenu();
        }


    }


}
