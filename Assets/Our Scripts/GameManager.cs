using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnitStats;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public List<GameObject> myCharacterPool = new List<GameObject>();
    public List<GameObject> enemyPool = new List<GameObject>();
    public int wave = 1;
    public float waveTimer;
    public int numberOfEnemies;

    public float timerForSound = 0f;

   

    [HideInInspector]
    public List<GameObject>[] groups = new List<GameObject>[10];
    public GameObject selectionMenu;
    [HideInInspector]
    public List<GameObject> knightSelected, archerSelected;
    [HideInInspector]
    public GameObject King;
    public GameObject Base;
    [HideInInspector]
    public bool baseSelected=false;

    public GameObject banner;

    public ParticleClick[] animations = new ParticleClick[3];

    private GameObject lastClick;
    private float lastTime;

    public List<Texture2D> cursors;

    [HideInInspector]
    public typeAction currentAction = typeAction.Normal;

    public GameObject[] enemies;
    public GameObject enemySpawnPosition;

    public GameObject objectivo;
    private Text objectivesText;


    public enum typeAction
    {
        Move,
        Normal,
        Attack,
        Stop,
        Banner
    }

    [SerializeField]
    private RectTransform selectSquareImage;

    bool activateSelectArea = false;
    bool UIclick = false;
    Vector3 startPos, endPos;
    [HideInInspector]
    public int silver;
    public GameObject silverText;
    private bool actionDone = false;

    private void Awake()
    {
        knightSelected = new List<GameObject>();
        archerSelected = new List<GameObject>();

        
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.SetCursor(cursors[2], new Vector2(0, 0), CursorMode.Auto);
        numberOfEnemies = 2;
        this.objectivesText = this.objectivo.GetComponent<Text>();
        waveTimer = 10;
        selectSquareImage.gameObject.SetActive(false);
        Base.GetComponent<Outline>().enabled = false;
        banner.active = false;
        silver = 1000;
    }

    // Update is called once per frame
    void Update()
    {
        if (timerForSound > 0)
        {
            timerForSound -= Time.fixedDeltaTime;
        }

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
                else {
                    switch (currentAction) {
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
            else {
                Cursor.SetCursor(cursors[2], new Vector2(0, 0), CursorMode.Auto);
            }
        }


        silverText.GetComponent<Text>().text = "Silver: " + silver;

        if (Input.GetMouseButtonDown(0))
        {

            RaycastHit hit;

            if (!EventSystem.current.IsPointerOverGameObject())
            {
                UIclick = false;
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
                            else {

                                playAttackSound();

                            }


                        }else if(hit.collider.tag == "MyUnit" && currentAction == typeAction.Move)
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
                            if (lastClick == hit.collider.gameObject.GetComponentInParent<UnitStats>().gameObject && Time.time-lastTime <=0.5f)
                            {
                                if (!shiftKeysDown())
                                {
                                    clearSelection();
                                }
                                Troops type = hit.collider.gameObject.GetComponent<UnitStats>().troop;

                                foreach ( GameObject go in myCharacterPool)
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
                else if(!actionDone)
                {

                    Rect selectRect = new Rect(squareStart.x, squareStart.y, endPos.x - squareStart.x, endPos.y - squareStart.y);
                    int count = 0;
                    foreach (GameObject go in myCharacterPool)
                    {
                        if (selectRect.Contains(Camera.main.WorldToScreenPoint(go.transform.position), true))
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

        else if (Input.GetMouseButton(0) && activateSelectArea )
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
                if (selectRect.Contains(Camera.main.WorldToScreenPoint(go.transform.position), true))
                {
                    go.GetComponentInParent<MovementManager>().hover = true;
                }
            }

        }


        if (Input.GetMouseButtonDown(1) && (knightSelected.Count > 0 || archerSelected.Count>0 || King!=null) && currentAction == typeAction.Normal)
        {

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
        }else if (Input.GetMouseButtonDown(1)){
            changeToNormal();
        }
        if ((knightSelected.Count > 0 || archerSelected.Count > 0 || King != null)) {
            if (Input.GetKey(KeyCode.A))
            {
                changeToAttack();
            }
            if (Input.GetKey(KeyCode.S))
            {
                makeAction(null, typeAction.Stop);
            }

            if (Input.GetKey(KeyCode.M))
            {
                changeToMove();
            }
        }

        getNumberKey();
        Spawn();
        objectivesText.text = "Objectives:\n" + "Wave: " + (wave-1) + "\n" + "Enemies: " + enemyPool.Count;

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

        foreach (GameObject go in list)
        {
            addToSelection(go);
        }
    }

    public void makeAction(Vector3 target, typeAction type)
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
                    King.GetComponentInParent<MovementManager>().currentAction = new Normal(target, King, this);
                    King.GetComponentInParent<MovementManager>().currentAction.Start();
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



    public void Spawn()
    {
 
        if(enemyPool == null || enemyPool.Count == 0)
        {
            if (waveTimer > 0)
            {
                waveTimer -= Time.deltaTime;
                return;
            }
            waveTimer = 10;
            wave++;
            if (wave <= 20)
            {
                numberOfEnemies += 3;
            }
            GameObject obj = enemies[0];
            for(int i = 0; i < numberOfEnemies/2; i++)
            {
                float x = UnityEngine.Random.Range(-10,10);
                float z = UnityEngine.Random.Range(-10, 10);
                enemySpawnPosition.transform.position = new Vector3(enemySpawnPosition.transform.position.x + x, enemySpawnPosition.transform.position.y, enemySpawnPosition.transform.position.z + z);
                GameObject instEnemy = Instantiate(obj,enemySpawnPosition.transform.position,Quaternion.identity);
               
            }
            obj = enemies[1];
            for (int i = 0; i < numberOfEnemies/2; i++)
            {
                float x = UnityEngine.Random.Range(-10, 10);
                float z = UnityEngine.Random.Range(-10, 10);
                enemySpawnPosition.transform.position = new Vector3(enemySpawnPosition.transform.position.x + x, enemySpawnPosition.transform.position.y, enemySpawnPosition.transform.position.z + z);
                GameObject instEnemy = Instantiate(obj, enemySpawnPosition.transform.position, Quaternion.identity);
               
            }
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
            selectGroupAction(0);
        }
        else if (Input.GetKey(KeyCode.Alpha2))
        {
            selectGroupAction(1);
        }
        else if (Input.GetKey(KeyCode.Alpha3))
        {
            selectGroupAction(2);
        }
        else if (Input.GetKey(KeyCode.Alpha4))
        {
            selectGroupAction(3);
        }
        else if (Input.GetKey(KeyCode.Alpha5))
        {
            selectGroupAction(4);
        }
        else if (Input.GetKey(KeyCode.Alpha6))
        {
            selectGroupAction(5);
        }
        else if (Input.GetKey(KeyCode.Alpha7))
        {
            selectGroupAction(6);
        }
        else if (Input.GetKey(KeyCode.Alpha8))
        {
            selectGroupAction(7);
        }
        else if (Input.GetKey(KeyCode.Alpha9))
        {
            selectGroupAction(8);
        }
        else if (Input.GetKey(KeyCode.Alpha0))
        {
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
            clearSelection();
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
}

