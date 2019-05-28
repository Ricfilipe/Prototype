using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnitStats;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [Header("Objects")]
    public List<GameObject> myCharacterPool = new List<GameObject>();
    public List<GameObject> enemyPool = new List<GameObject>();
    public GameObject[] enemySpawnPosition;
    public GameObject Base;
    public GameObject baseDestroyer;

    [Header("State")]
    public int wave = 0;
    public float waveTimer;
    public int numberOfEnemies;
    public int[] wave_numbers = new int[9];
    public int indice = 0;
    public float wave_lenght = 0;

    [HideInInspector]
    private bool paused=false;
   
    public float timerForSound = 0f;
    [HideInInspector]
    public VoiceManager messenger;

    [HideInInspector]
    public List<GameObject>[] groups = new List<GameObject>[10];

    [HideInInspector]
    public List<GameObject> knightSelected, archerSelected;
    [HideInInspector]
    public GameObject King;
    [HideInInspector]
    public bool baseSelected=false;

    [HideInInspector]
    public bool baseDestroyed = false;

    private GameObject lastClick;
    private float lastTime;

    private bool waitForDestroy;

    [HideInInspector]
    public typeAction currentAction = typeAction.Normal;



    [Header("HUD")]
    public GameObject pauseMenu;
    public GameObject endingPanel;
    public GameObject endingText;
    private Text ending;
    public GameObject selectionMenu;
    [SerializeField]
    private RectTransform selectSquareImage;
    public GameObject objectivo;
    public GameObject silverText;
    public List<Texture2D> cursors;
    public ParticleClick[] animations = new ParticleClick[3];
    public GameObject banner;
    private Text objectivesText;
    private bool nodescription;

    [Header("Waves")]
    public GameObject[] wave1;
    public GameObject[] wave2;
    public GameObject[] wave3;
    public GameObject[] wave4;
    public GameObject[] wave5;
    public GameObject[] wave6;
    public GameObject[] wave7;
    public GameObject[] wave8;
    public GameObject[] wave9;

    private GameObject[][] waves;

    [Header("Metrics")]
    public Metrics metrics;


    public enum typeAction
    {
        Move,
        Normal,
        Attack,
        Stop,
        Banner
    }



    bool activateSelectArea = false;
    bool UIclick = false;
    Vector3 startPos, endPos;
    [HideInInspector]
    public int silver;
  
    private bool actionDone = false;

    private void Awake()
    {
        Time.timeScale = 1.0f;
        knightSelected = new List<GameObject>();
        archerSelected = new List<GameObject>();
        messenger= GetComponent<VoiceManager>();
        getWaves();
    }

    // Start is called before the first frame update
    void Start()
    {
        
        ending = endingText.GetComponent<Text>();
        Cursor.SetCursor(cursors[2], new Vector2(0, 0), CursorMode.Auto);
        numberOfEnemies = 2;
        this.objectivesText = this.objectivo.GetComponent<Text>();
        waveTimer = 10;
        selectSquareImage.gameObject.SetActive(false);
        Base.GetComponent<Outline>().enabled = false;
        banner.active = false;
        silver = 100;
        int[] a = { 1,0,0, 0,1,0, 0,0,1, 2,0,0, 1,1,1 };
        wave_numbers = a;
        MessageSequence seq = new MessageSequence();
        seq.list.Add(new SingleMessage("Henry V", " The french are well arrayed for battle.", ""));
        seq.list.Add(new SingleMessage("Soldier", "There's five to one! O that we now had here but one ten thousand of those men in England that do no work to-day", "Infantaria"));
        seq.list.Add(new SingleMessage("Henry V", "Fear not, if we are marked to die, we are enough to do our country loss. and if to live, the fewer men the greater share of honor. This day is called the feast of Crispian?", "Rei"));
        seq.list.Add(new SingleMessage("Soldier", "Aye, my lord.", "Infantaria"));
        seq.list.Add(new SingleMessage("Henry V", " He that shall live this day, and see old age, will yearly on the vigil feast his neighbors, and say \"Tomorrow is Saint Crispian\".", "Rei"));
        seq.list.Add(new SingleMessage("Henry V", " Then he will strip his sleeve and show his scars and say, \"These wounds I had on Crispin's Day\". The story shall the good man teach his son;", "Rei"));
        seq.list.Add(new SingleMessage("Henry V", "And Crispin Crispian shall ne'er go by, from this day to the ending of the world, but we in it shall be remembered - we few, we happy few, we band of brothers!", "Rei"));
        seq.list.Add(new SingleMessage("Soldier", "God's will my liege! Would that you and I alone without more help, could fight this royal battle!", "Infantaria"));
        seq.list.Add(new SingleMessage("Henry V", " Why, now thou hast unwish'd five thousand more men; which likes me better than to wish us one. The men know their places. God be with us all!", "Rei"));

        messenger.addToQueue(seq);
        
    }

    // Update is called once per frame
    void Update()
    {
      if(baseDestroyed)
        {
            Base.active = false;
        }

        if (timerForSound > 0)
        {
            timerForSound -= Time.fixedDeltaTime;
        }
        if (!paused)
        {
            RaycastHit hitOut;
            if (!activateSelectArea)
            {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitOut, 300))
                {

                    if (hitOut.collider.tag == "Enemy")
                    {
                        hitOut.collider.gameObject.GetComponent<Enemies>().hover = true;
                        Cursor.SetCursor(cursors[3], new Vector2(16, 16), CursorMode.ForceSoftware);
                    }
                    else if (hitOut.collider.tag == "MyUnit")
                    {
                        hitOut.collider.gameObject.GetComponentInParent<MovementManager>().hover = true;
                        if (currentAction == typeAction.Normal)
                        {
                            Cursor.SetCursor(cursors[4], new Vector2(16, 16), CursorMode.ForceSoftware);
                        }
                    }
                    else
                    {
                        switch (currentAction)
                        {
                            case typeAction.Attack:
                                Cursor.SetCursor(cursors[0], new Vector2(0, 0), CursorMode.ForceSoftware);
                                break;
                            case typeAction.Move:
                                Cursor.SetCursor(cursors[1], new Vector2(0, 0), CursorMode.ForceSoftware);
                                break;
                            default:
                                Cursor.SetCursor(cursors[2], new Vector2(0, 0), CursorMode.ForceSoftware);
                                break;
                        }


                    }



                }
                else
                {
                    Cursor.SetCursor(cursors[2], new Vector2(0, 0), CursorMode.Auto);
                }
            }


            silverText.GetComponent<Text>().text = "Moral: " + silver;

            if (Input.GetMouseButtonDown(0))
            {

                RaycastHit hit;

                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    UIclick = false;
                    metrics.IncActions();
                    if (currentAction == typeAction.Normal)
                    {

                        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 300))
                        {

                           
                            activateSelectArea = true;
                            startPos = hit.point;



                        }
                    }
                    else
                    {
                        actionDone = true;
                        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 300))
                        {

                            if (currentAction == typeAction.Banner)
                            {
                                banner.transform.position = new Vector3(hit.point.x, banner.transform.position.y, hit.point.z);
                                changeToNormal();

                            }
                            if (hit.collider.tag == "Enemy")
                            {
                                makeAction(hit.collider.gameObject, currentAction);
                                changeToNormal();


                                if (currentAction == typeAction.Move)
                                {
                                    playMoveSound();
                                }
                                else
                                {

                                    playAttackSound();

                                }


                            }
                            else if (hit.collider.tag == "MyUnit" && currentAction == typeAction.Move)
                            {
                                playMoveSound();
                                makeAction(hit.collider.gameObject, currentAction);
                                changeToNormal();
                            }
                            else
                            {
                                if (currentAction == typeAction.Attack)
                                {

                                    playAttackSound();

                                    Vector3 helper = hit.point;
                                    StartCoroutine(animations[1].click(helper));

                                    float min = 60f;
                                    int idx = -1;
                                    for (int i = 0; i < enemyPool.Count; i++)
                                    {
                                        float tempMin = (enemyPool[i].transform.position - helper).magnitude;
                                        if (tempMin <= min)
                                        {
                                            min = tempMin;
                                            idx = i;
                                        }
                                    }
                                    if (idx != -1)
                                    {
                                        makeAction(enemyPool[idx], currentAction);
                                        changeToNormal();
                                    }
                                    else
                                    {
                                        makeAction(hit.point, currentAction);
                                        changeToNormal();
                                    }
                                }
                                else
                                {
                                    playAttackSound();
                                    makeAction(hit.point, currentAction);
                                    changeToNormal();
                                    StartCoroutine(animations[2].click(hit.point));
                                }
                            }
                        }
                    }
                }
                else
                {
                    UIclick = true;
                }


            }
            else if (Input.GetMouseButtonUp(0) && !UIclick)
            {
                activateSelectArea = false;

                if (currentAction == typeAction.Normal && !actionDone)
                {
                    Vector3 squareStart = Camera.main.WorldToScreenPoint(startPos);
                    if (Math.Abs(endPos.x - squareStart.x) < 0.5 && Math.Abs(endPos.y - squareStart.y) < 0.5)
                    {
                        RaycastHit hit;
                        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 300))
                        {

                            if (hit.collider.tag == "MyUnit")
                            {
                                if (lastClick == hit.collider.gameObject.GetComponentInParent<UnitStats>().gameObject && Time.time - lastTime <= 0.5f)
                                {
                                    if (!shiftKeysDown())
                                    {
                                        clearSelection();
                                    }
                                    Troops type = hit.collider.gameObject.GetComponent<UnitStats>().troop;

                                    foreach (GameObject go in myCharacterPool)
                                    {
                                        if (go.GetComponent<UnitStats>().troop == type)
                                        {
                                            addToSelection(go);
                                        }
                                    }
                                }
                                else
                                {
                                    lastClick = hit.collider.gameObject.GetComponentInParent<UnitStats>().gameObject;
                                    lastTime = Time.time;
                                    if (shiftKeysDown())
                                    {

                                        addOrRemoveFromSelection(hit);
                                        baseSelected = false;
                                        Base.GetComponentInParent<Outline>().enabled = false;
                                        banner.active = false;

                                    }
                                    else
                                    {
                                        clearSelection();
                                        addToSelection(hit);
                                        baseSelected = false;
                                        Base.GetComponentInParent<Outline>().enabled = false;
                                        banner.active = false;
                                    }
                                }
                            }
                            else if (hit.collider.tag == "Base")
                            {
                                clearSelection();
                                baseSelected = true;
                                Base.GetComponentInParent<Outline>().enabled = true;
                                banner.active = true;
                            }

                        }

                    }
                    else if (!actionDone)
                    {
                        metrics.IncActions();
                        Rect selectRect = new Rect(squareStart.x, squareStart.y, endPos.x - squareStart.x, endPos.y - squareStart.y);
                        int count = 0;
                        foreach (GameObject go in myCharacterPool)
                        {
                            if (selectRect.Contains(Camera.main.WorldToScreenPoint(go.GetComponentInChildren<NavMeshAgent>().transform.position), true))
                            {
                                if (count == 0)
                                {
                                    if (!shiftKeysDown())
                                    {
                                        clearSelection();
                                    }
                                    baseSelected = false;
                                    Base.GetComponentInParent<Outline>().enabled = false;
                                    banner.active = false;
                                    count++;

                                }
                                addToSelection(go);
                            }
                        }
                    }

                    selectSquareImage.gameObject.SetActive(false);
                }
                if (actionDone)
                    actionDone = false;
            }

            else if (Input.GetMouseButton(0) && activateSelectArea)
            {

                if (!selectSquareImage.gameObject.activeInHierarchy)
                {
                    selectSquareImage.gameObject.SetActive(true);
                }

                endPos = Input.mousePosition;
                Vector3 squareStart = Camera.main.WorldToScreenPoint(startPos);
                squareStart.z = 0f;

                Vector3 centre = (squareStart + endPos) / 2f;

                selectSquareImage.position = centre;

                float sizex = Mathf.Abs(squareStart.x - endPos.x);
                float sizey = Mathf.Abs(squareStart.y - endPos.y);

                selectSquareImage.sizeDelta = new Vector2(sizex, sizey);

                Rect selectRect = new Rect(squareStart.x, squareStart.y, endPos.x - squareStart.x, endPos.y - squareStart.y);
                foreach (GameObject go in myCharacterPool)
                {
                    if (selectRect.Contains(Camera.main.WorldToScreenPoint(go.GetComponentInChildren<NavMeshAgent>().transform.position), true))
                    {
                        go.GetComponentInParent<MovementManager>().hover = true;
                    }
                }

            }


            if (Input.GetMouseButtonDown(1) && (knightSelected.Count > 0 || archerSelected.Count > 0 || King != null) && currentAction == typeAction.Normal)
            {
                metrics.IncActions();
                RaycastHit hit;

                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 300))
                {
                    if (hit.collider.tag == "Enemy")
                    {
                        makeAction(hit.collider.gameObject, typeAction.Normal);
                        playAttackSound();
                    }
                    else
                    {
                        makeAction(hit.point, typeAction.Normal);
                        StartCoroutine(animations[0].click(hit.point));
                        playMoveSound();
                    }

                }
            }
            else if (Input.GetMouseButtonDown(1))
            {

                changeToNormal();
            }
            if ((knightSelected.Count > 0 || archerSelected.Count > 0 || King != null))
            {
                if (Input.GetKeyDown(KeyCode.A))
                {
                    metrics.IncActions();
                    changeToAttack();
                }
                if (Input.GetKeyDown(KeyCode.S))
                {
                    metrics.IncActions();
                    makeAction(null, typeAction.Stop);
                }

                if (Input.GetKeyDown(KeyCode.M))
                {
                    metrics.IncActions();
                    changeToMove();
                }
            }

            getNumberKey();
            Spawn();
            objectivesText.text = "Objectives:\n" + "Wave: " + (wave) + "\n" + "Enemies: " + enemyPool.Count;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (paused)
            {
                UnPause();
            }
            else
            {
                Pause();
            }
        }
    }

    public void clearSelection()
    {

        foreach (GameObject go in archerSelected)
        {
            go.GetComponent<MovementManager>().selected = false;
        }
        foreach (GameObject go in knightSelected)
        {
            go.GetComponent<MovementManager>().selected = false;
        }
        if (King != null)
        {
            King.GetComponentInParent<MovementManager>().selected = false;
            King = null;
        }
        archerSelected.Clear();
        knightSelected.Clear();
        selectionMenu.GetComponent<generateSelection>().change = true;
        selectionMenu.GetComponent<generateSelection>().seq = 0;
    }




    public void addToSelection( RaycastHit hit) {
        hit.collider.GetComponentInParent<MovementManager>().selected = true;
        addToSelection(hit.collider.gameObject.GetComponentInParent<UnitStats>().gameObject);
    }

     public void addOrRemoveFromSelection( RaycastHit hit) {
       
        addOrRemoveFromSelection(hit.collider.gameObject.GetComponentInParent<UnitStats>().gameObject);
        selectionMenu.GetComponent<generateSelection>().change = true;
    }


    public void addOrRemoveFromSelection(GameObject go)
    {

        Troops type = go.GetComponent<UnitStats>().troop;
        switch (type)
        {
            case Troops.Archer:
                if (!archerSelected.Remove(go))
                {
                    archerSelected.Add(go);
                    go.GetComponent<MovementManager>().selected = true;
                }
                else {
                    go.GetComponent<MovementManager>().selected = false;
                }
                
                break;

            case Troops.Infantry:
                if (!knightSelected.Remove(go))
                {
                    knightSelected.Add(go);
                    go.GetComponentInParent<MovementManager>().selected = true;
                }
                else
                {
                    go.GetComponentInParent<MovementManager>().selected = false;
                }
                break;

            case Troops.King:
                if (King != null)
                {
                    King = null;
                    go.GetComponentInParent<MovementManager>().selected = false;
                }
                else
                {
                    King = go;
                    go.GetComponentInParent<MovementManager>().selected = true;
                }
                break;
        }


     
    }



    public void addToSelection(GameObject go)
    {
        if(go == null)
        {
            return;
        }

        Troops type = go.GetComponentInParent<UnitStats>().troop;
        switch (type)
        {
            case Troops.Archer:
                if(!archerSelected.Contains(go))
                archerSelected.Add(go);
                selectionMenu.GetComponent<generateSelection>().change = true;
                break;

            case Troops.Infantry:
                if (!knightSelected.Contains(go))
                    knightSelected.Add(go);
                selectionMenu.GetComponent<generateSelection>().change = true;
                break;

            case Troops.King:
                King = go;
                selectionMenu.GetComponent<generateSelection>().change = true;
                break;
        }
        

        go.GetComponentInParent<MovementManager>().selected = true;
    }


    public void removeFromSelection(GameObject go)
    {

        Troops type = go.GetComponent<UnitStats>().troop;
        switch (type)
        {
            case Troops.Archer:
                archerSelected.Remove(go);
                break;

            case Troops.Infantry:
                knightSelected.Remove(go);
                break;

            case Troops.King:
                King = null;
                break;
        }
        selectionMenu.GetComponent<generateSelection>().change = true;
        go.GetComponent<MovementManager>().selected = false;
    }

    public void addGroup(int index)
    {

        List<GameObject> list = new List<GameObject>();
        list.AddRange(archerSelected);
        list.AddRange(knightSelected);

        if (King != null)
        {
            list.Add(King);
        }
        

        groups[index]=list;
    }

    public void selectGroup(int index)
    {
        List<GameObject> list = groups[index];
        clearSelection();
        
        foreach (GameObject go in list)
        {
            addToSelection(go);
        }
    }

    public void makeAction(Vector3 target, typeAction type)
    {
        Vector3 offset = new Vector3(0,0,0);
        if (King != null)
        {

            offset.x += 3f;

            DoMovement(target, this.King, type);
            King.GetComponentInParent<MovementManager>().currentAction.Start();
        }
        if (this.knightSelected.Count > 0)
        {
            
            var numlin = this.knightSelected.Count / 10;
            int i=0;
            for(; i < numlin; i++)
            {
                offset.z = 13.5f;
                DoMovement(target + offset, this.knightSelected[i * 10], type);                     
                offset.z = 10.5f;
                DoMovement(target + offset, this.knightSelected[i * 10+1], type);
                offset.z = 7.5f;
                DoMovement(target + offset, this.knightSelected[i * 10+2], type);
                offset.z = 4.5f;
                DoMovement(target + offset, this.knightSelected[i * 10+3], type);
                offset.z = 1.5f;
                DoMovement(target + offset, this.knightSelected[i * 10+4], type);
                offset.z = -1.5f;
                DoMovement(target + offset, this.knightSelected[i * 10+5], type);
                offset.z = -4.5f;
                DoMovement(target + offset, this.knightSelected[i * 10+6], type);
                offset.z = -7.5f;
                DoMovement(target + offset, this.knightSelected[i * 10+7], type);
                offset.z = -10.5f;
                DoMovement(target + offset, this.knightSelected[i * 10+8], type);
                offset.z = -13.5f;
                DoMovement(target + offset, this.knightSelected[i * 10 + 9], type);
                offset.x += 3f;
            }
            var lastlin = knightSelected.Count - (i * 10);
            offset.z = 1.5f;
            for (int j = i * 10; j < knightSelected.Count;j++) {
                if (lastlin % 2 == 0)
                {
                    if (j%2==0)
                    {
                        DoMovement(target + offset, this.knightSelected[j], type);
                    }
                    else
                    {
                        offset.z = -offset.z;
                        DoMovement(target + offset, this.knightSelected[j], type);
                        offset.z = (-offset.z) + 3f;
                    }

                }
                else
                {
                    if (j - i * 10 == 0)
                    {

                        DoMovement(target + new Vector3(offset.x,0,0), this.knightSelected[j], type);
                        offset.z = offset.z * 2;
                    }
                    else if (j % 2 == 0)
                    {
                        offset.z = -offset.z;
                        DoMovement(target + offset, this.knightSelected[j], type);
                        offset.z = (offset.z) + 3f;
                    }
                 
                    else
                    {
                        offset.z = -offset.z;
                        DoMovement(target + offset, this.knightSelected[j], type);
                      
                    }
                }
            }
            if (lastlin > 0)
            {
                offset.x += 3f;
            }

        }
        if (this.archerSelected.Count > 0)
        {
            var numlin = this.archerSelected.Count / 8;
            int i = 0;
            for (; i < numlin; i++)
            {
                offset.z = 10.5f;
                DoMovement(target + offset, this.archerSelected[i * 8], type);
                offset.z = 7.5f;
                DoMovement(target + offset, this.archerSelected[i * 8 + 1], type);
                offset.z = 4.5f;
                DoMovement(target + offset, this.archerSelected[i * 8 + 2], type);
                offset.z = 1.5f;
                DoMovement(target + offset, this.archerSelected[i * 8 + 3], type);
                offset.z = -1.5f;
                DoMovement(target + offset, this.archerSelected[i * 8 + 4], type);
                offset.z = -4.5f;
                DoMovement(target + offset, this.archerSelected[i * 8 + 5], type);
                offset.z = -7.5f;
                DoMovement(target + offset, this.archerSelected[i * 8 + 6], type);
                offset.z = -10.5f;
                DoMovement(target + offset, this.archerSelected[i * 8 + 7], type);

                offset.x += 3f;
            }
            var lastlin = archerSelected.Count - (i * 8);
            offset.z = 1.5f;
            for (int j = i * 8; j < archerSelected.Count; j++)
            {
                if (lastlin % 2 == 0)
                {
                    if (j % 2 == 0)
                    {

                        DoMovement(target + offset, this.archerSelected[j], type);
                    }
                    else
                    {
                        offset.z = -offset.z;
                        DoMovement(target + offset, this.archerSelected[j], type);
                        offset.z = (-offset.z) + 3f;
                    }

                }
                else
                {
                    if (j - i * 8 == 0)
                    {

                        DoMovement(target + new Vector3(offset.x, 0, 0), this.archerSelected[j], type);
                        offset.z = offset.z * 2;
                    }
                    else if (j % 2 == 0)
                    {
                        offset.z = -offset.z;
                        DoMovement(target + offset, this.archerSelected[j], type);
                        offset.z = (offset.z) + 3f;
                    }
                    else
                    {
                        offset.z = -offset.z;
                        DoMovement(target + offset, this.archerSelected[j], type);
                        
                    }
                }
            }
        }

    }



    public void Spawn()
    {
 
        if(enemyPool == null || enemyPool.Count == 0)
        {
            if (wave_lenght != 0 && indice!=0)
            {
                metrics.addWaveTimer(wave_lenght, indice / 3);
                wave_lenght = 0;
            }
            if (waveTimer > 0)
            {
                if (messenger.isEmpty())
                {
                    waveTimer -= Time.deltaTime;
                }
                return;
            }
            
            
            if (wave  == waves.Length-1 && !baseDestroyed)
            {
                if (!waitForDestroy)
                {
                    messenger.addToQueue(new TimedMessage(10, "Soldier", "My king, the french are attacking our camp, we won't be able to call reinforcements or upgrade our units, after the attack", "Infantaria"));
                    messenger.addToQueue(new TimedMessage(20, "King","This is going to be our Last Stand, prepare yourselves", "Rei"));                   
                    waitForDestroy = true;
                    waveTimer = 1;
                    wave_lenght += 30;
                    return;
                }
                else
                {
                  
                    GameObject.Instantiate(baseDestroyer,new Vector3(-284.5f,0,-208.5f),new Quaternion());
                    wave++;
                    waveTimer = 34;
                   
                }
            }
            else
            {
                wave++;
                waveTimer = 10;
                wave_lenght += waveTimer;
            }

            if (wave-1 >= waves.Length)
            {
                Victory();
                metrics.setWin();
                metrics.addWaveTimer(wave_lenght, waves.Length-1);
                metrics.toFile();
            }
            else
            {
                int i = 0;
                foreach( GameObject w in waves[wave-1])
                {

                    GameObject.Instantiate(w, enemySpawnPosition[i].transform.position, Quaternion.identity);
                    i++;
                }
               
                if (wave > 1) {
                    metrics.addWaveTimer(wave_lenght,wave-1);
                }
                wave_lenght = 0;
            }
        }
        else
        {
            wave_lenght += Time.deltaTime;
        }
    }

    public void makeAction(GameObject target, typeAction type)
    {
        switch (type)
        {
            case typeAction.Normal:
                foreach (GameObject go in this.knightSelected)
                {
                    go.GetComponent<MovementManager>().currentAction = new Normal(target, go, this);
                    go.GetComponent<MovementManager>().currentAction.Start();
                }
                foreach (GameObject go in this.archerSelected)
                {
                    go.GetComponent<MovementManager>().currentAction = new Normal(target, go, this);
                    go.GetComponent<MovementManager>().currentAction.Start();

                }
                if (King != null)
                {
                    King.GetComponent<MovementManager>().currentAction = new Normal(target, King, this);
                    King.GetComponent<MovementManager>().currentAction.Start();
                }
                break;

            case typeAction.Attack:
                foreach (GameObject go in this.knightSelected)
                {
                    go.GetComponent<MovementManager>().currentAction = new Attack(target, go, this);
                    go.GetComponent<MovementManager>().currentAction.Start();

                }
                foreach (GameObject go in this.archerSelected)
                {
                    go.GetComponent<MovementManager>().currentAction = new Attack(target, go, this);
                    go.GetComponent<MovementManager>().currentAction.Start();

                }
                if (King != null)
                {
                    King.GetComponent<MovementManager>().currentAction = new Attack(target, King, this);
                    King.GetComponent<MovementManager>().currentAction.Start();
                }
                break;

            case typeAction.Move:
                foreach (GameObject go in this.knightSelected)
                {
                    go.GetComponent<MovementManager>().currentAction = new Move(target, go, this);
                    go.GetComponent<MovementManager>().currentAction.Start();

                }
                foreach (GameObject go in this.archerSelected)
                {
                    go.GetComponent<MovementManager>().currentAction = new Move(target, go, this);
                    go.GetComponent<MovementManager>().currentAction.Start();

                }
                if (King != null)
                {
                    King.GetComponent<MovementManager>().currentAction = new Move(target, King, this);
                    King.GetComponent<MovementManager>().currentAction.Start();
                }
                break;

            case typeAction.Stop:
                foreach (GameObject go in this.knightSelected)
                {
                    go.GetComponent<MovementManager>().currentAction = new Stop(target, go, this);
                    go.GetComponent<MovementManager>().currentAction.Start();

                }
                foreach (GameObject go in this.archerSelected)
                {
                    go.GetComponent<MovementManager>().currentAction = new Stop(target, go, this);
                    go.GetComponent<MovementManager>().currentAction.Start();

                }
                if (King != null)
                {
                    King.GetComponent<MovementManager>().currentAction = new Stop(target, King, this);
                    King.GetComponent<MovementManager>().currentAction.Start();
                }
                break;

        }

    }

    public static bool shiftKeysDown()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
    public bool ctrlKeysDown()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            return true;
        }
        else
        {
            return false;
        }
    }





    public void getNumberKey()
    {
        

        if (Input.GetKey(KeyCode.Alpha1))
        {
            metrics.IncActions();
            selectGroupAction(0);
        }
        else if (Input.GetKey(KeyCode.Alpha2))
        {
            metrics.IncActions();
            selectGroupAction(1);
        }
        else if (Input.GetKey(KeyCode.Alpha3))
        {
            metrics.IncActions();
            selectGroupAction(2);
        }
        else if (Input.GetKey(KeyCode.Alpha4))
        {
            metrics.IncActions();
            selectGroupAction(3);
        }
        else if (Input.GetKey(KeyCode.Alpha5))
        {
            metrics.IncActions();
            selectGroupAction(4);
        }
        else if (Input.GetKey(KeyCode.Alpha6))
        {
            metrics.IncActions();
            selectGroupAction(5);
        }
        else if (Input.GetKey(KeyCode.Alpha7))
        {
            metrics.IncActions();
            selectGroupAction(6);
        }
        else if (Input.GetKey(KeyCode.Alpha8))
        {
            metrics.IncActions();
            selectGroupAction(7);
        }
        else if (Input.GetKey(KeyCode.Alpha9))
        {
            metrics.IncActions();
            selectGroupAction(8);
        }
        else if (Input.GetKey(KeyCode.Alpha0))
        {
            metrics.IncActions();
            selectGroupAction(9);
        }
       
    }

    public void changeToAttack()
    {
        Cursor.SetCursor(cursors[0], new Vector2(0f, 0f), CursorMode.ForceSoftware);
        currentAction = typeAction.Attack;
    }

    public void changeToNormal()
    {
        Cursor.SetCursor(cursors[2], new Vector2(0, 0), CursorMode.ForceSoftware);
        currentAction = typeAction.Normal;
    }

    public void changeToMove()
    {
        Cursor.SetCursor(cursors[1], new Vector2(0f, 0f), CursorMode.ForceSoftware);
        currentAction = typeAction.Move;
    }

    public void selectGroupAction(int idx)
    {
        if (groups[idx] == null)
        {
            groups[idx] = new List<GameObject>();
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            List<GameObject> currentGroup = groups[idx];


            foreach(GameObject go in archerSelected)
            {
                if (!currentGroup.Contains(go))
                    currentGroup.Add(go);
            }

            foreach (GameObject go in knightSelected)
            {

                if (!currentGroup.Contains(go))
                    currentGroup.Add(go);
            }

            if(King != null)
            {
                if (!currentGroup.Contains(King))
                    currentGroup.Add(King);
            }

        }
        else if (Input.GetKey(KeyCode.LeftControl)) {
            
            addGroup(idx);
        }
        else {
       
            selectGroup(idx);
        }

    }

    public void AddSilver(int amount)
    {
        silver += amount;
    }


    private void playSpecificMoveSound(GameObject go)
    {
        AudioSource[] sound = go.GetComponent<MovementManager>().movingSpecific;
        sound[Random.Range(0, sound.Length)].Play();
    }

    private void playGenericMoveSound(GameObject go)
    {
        AudioSource[] sound = go.GetComponent<MovementManager>().movingGeneric;
        sound[Random.Range(0, sound.Length)].Play();
    }

    private void playSpecificAttackSound(GameObject go)
    {
        AudioSource[] sound = go.GetComponent<MovementManager>().attackSpecific;
        sound[UnityEngine.Random.Range(0, sound.Length)].Play();

    }

    private void playGenericAttackSound(GameObject go)
    {
        AudioSource[] sound = go.GetComponent<MovementManager>().attackingGeneric;
        sound[UnityEngine.Random.Range(0, sound.Length)].Play();
    }

    private void playAttackSound() {
        if (timerForSound <= 0)
        {
            timerForSound = 3f;
            if (King == null && archerSelected.Count == 0)
            {
                playSpecificAttackSound(knightSelected[0]);
            }
            else if (King == null && knightSelected.Count == 0)
            {
                playSpecificAttackSound(archerSelected[0]);
            }
            else if (knightSelected.Count == 0 && archerSelected.Count == 0)
            {
                playSpecificAttackSound(King);
            }
            else
            {
                if (archerSelected.Count == 0)
                {
                    playGenericAttackSound(knightSelected[0]);
                }
                else
                {
                    playGenericAttackSound(archerSelected[0]);
                }
            }
        }
    }

    private void playMoveSound()
    {
        if (timerForSound <= 0)
        {
            timerForSound = 3f;
            if (King == null && archerSelected.Count == 0)
            {
                playSpecificMoveSound(knightSelected[0]);
            }
            else if (King == null && knightSelected.Count == 0)
            {
                playSpecificMoveSound(archerSelected[0]);
            }
            else if (knightSelected.Count == 0 && archerSelected.Count == 0)
            {
                playSpecificMoveSound(King);
            }
            else
            {
                if (archerSelected.Count == 0)
                {
                    playGenericMoveSound(knightSelected[0]);
                }
                else
                {
                    playGenericMoveSound(archerSelected[0]);
                }
            }
        }
    }


    private void DoMovement(Vector3 target, GameObject go, typeAction type)
    {
        switch (type)
        {
            case typeAction.Normal:
                go.GetComponent<MovementManager>().currentAction = new Normal(target, go, this);
                break;
            case typeAction.Attack:
                go.GetComponent<MovementManager>().currentAction = new Attack(target, go, this);
                break;
            case typeAction.Move:
                go.GetComponent<MovementManager>().currentAction = new Move(target, go, this);
                break;
            case typeAction.Stop:
                go.GetComponent<MovementManager>().currentAction = new Stop(target, go, this);
                break;
        }
        go.GetComponentInChildren<MovementManager>().currentAction.Start();
    }

    private void Victory()
    {
        ending.text = "Victory";
        endingPanel.active = true;
        Time.timeScale = 0f;
    }

    public void Defeat()
    {
        
        metrics.addWaveTimer(wave_lenght, wave );
        ending.text = "Defeat";
        endingPanel.active = true;
        Time.timeScale = 0f;
        metrics.toFile();
    }

    private void Pause()
    {
        if (Time.timeScale != 0f)
        {
           
            pauseMenu.active = true;
            paused = true;
            activateSelectArea = false;
            selectSquareImage.gameObject.SetActive(false);
            Time.timeScale = 0f;
        }
    }

    public void UnPause()
    {
        Time.timeScale = 1f;
        pauseMenu.active = false ;
        paused = false;
    }

    public void Restart()
    {
        SceneManager.LoadScene("scene");
    }

    public void toMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        
    }


    public int  getWaves()
    {

        string path = "Assets/config.txt";
        StreamReader reader = new StreamReader(path);
        string strWaves = reader.ReadToEnd();
        int n_waves = int.Parse(strWaves);
        Debug.Log(n_waves);
        switch (n_waves)
        {
            case 4:
                waves = new GameObject[][] { wave1, wave2, wave8, wave9 };
                break;
            case 5:
                waves = new GameObject[][] { wave1, wave2, wave3, wave8, wave9 };
                break;
            case 6:
                waves = new GameObject[][] { wave1, wave2, wave3, wave4, wave8, wave9 };
                break;
            case 7:
                waves = new GameObject[][] { wave1, wave2, wave3, wave4, wave5, wave8, wave9 };
                break;
            case 8:
                waves = new GameObject[][] { wave1, wave2, wave3, wave4, wave5, wave6, wave8, wave9 };
                break;
            case 9:
                waves = new GameObject[][] { wave1, wave2, wave3, wave4, wave5, wave6, wave7, wave8, wave9 };
                break;
        }
        return n_waves;
    }

}

