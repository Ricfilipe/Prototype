using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnitStats;

public class GameManager : MonoBehaviour
{
    public List<GameObject> myCharacterPool = new List<GameObject>();
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

    public enum typeAction
    {
        Move,
        Normal,
        Attack
    }

    [SerializeField]
    private RectTransform selectSquareImage;

    bool activateSelectArea = false;
    bool UIclick = false;
    Vector3 startPos, endPos;
    [HideInInspector]
    public int silver;
    public GameObject silverText;

    private void Awake()
    {
        knightSelected = new List<GameObject>();
        archerSelected = new List<GameObject>();

        
    }

    // Start is called before the first frame update
    void Start()
    {
        selectSquareImage.gameObject.SetActive(false);
        Base.GetComponent<Outline>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        silverText.GetComponent<Text>().text = "Silver: " + silver;

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
           
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                UIclick = false;
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 300))
                {
                    
                    activateSelectArea = true;
                    startPos = hit.point;
                   

                }
            }
            else {
                UIclick = true;
            }
        }
        else if (Input.GetMouseButtonUp(0) && !UIclick)
        {
            activateSelectArea = false;
           

            Vector3 squareStart = Camera.main.WorldToScreenPoint(startPos);
            if (Math.Abs(endPos.x - squareStart.x) < 0.5 && Math.Abs(endPos.y - squareStart.y) < 0.5)
            {
                RaycastHit hit;
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 300))
                {

                    if (hit.collider.tag == "MyUnit")
                    {
                        if (shiftKeysDown())
                        {
                            addToSelection(hit);
                            baseSelected = false;
                            Base.GetComponent<Outline>().enabled = false;
                        }
                        else
                        {
                            clearSelection();
                            addToSelection(hit);
                            baseSelected = false;
                            Base.GetComponent<Outline>().enabled = false;
                        }
                    }
                    else if (hit.collider.tag == "Base")
                    {
                        if (shiftKeysDown())
                        {
                            baseSelected = true;
                            Base.GetComponent<Outline>().enabled = true;
                            
                        }
                        else
                        {
                            clearSelection();
                            baseSelected = true;
                            Base.GetComponent<Outline>().enabled = true;
                        }
                    }
                }

            }
            else
            {

                Rect selectRect = new Rect(squareStart.x, squareStart.y, endPos.x - squareStart.x, endPos.y - squareStart.y);
                int count = 0;
                foreach (GameObject go in myCharacterPool)
                {
                    if (selectRect.Contains(Camera.main.WorldToScreenPoint(go.transform.position), true))
                    {
                        if (count == 0) {
                            if (shiftKeysDown())
                            {
                                baseSelected = false;
                                Base.GetComponent<Outline>().enabled = false;
                                count++;
                            }
                            else
                            {
                                clearSelection();
                                baseSelected = false;
                                Base.GetComponent<Outline>().enabled = false;
                                count++;
                            }
                        }
                        addToSelection(go);
                    }
                }
            }
            
            selectSquareImage.gameObject.SetActive(false);

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
        }


        if (Input.GetMouseButtonDown(1) && (knightSelected.Count > 0 || archerSelected.Count>0 || King!=null))
        {

            RaycastHit hit;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 300))
            {
                if (hit.collider.tag == "Enemie")
                {
                    makeAction(hit.collider.gameObject, typeAction.Normal);
                }
                else
                {
                    makeAction(hit.point, typeAction.Normal);

                }

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
            King.GetComponent<MovementManager>().selected = false;
            King = null;
        }
        archerSelected.Clear();
        knightSelected.Clear();
        selectionMenu.GetComponent<generateSelection>().change = true;
    }



    public void addToSelection( RaycastHit hit) {
        hit.collider.GetComponent<MovementManager>().selected = true;
        addToSelection(hit.collider.gameObject);
    }

    public void addToSelection(GameObject go)
    {

        Troops type = go.GetComponent<UnitStats>().troop;
        switch (type)
        {
            case Troops.Archer:
                archerSelected.Add(go);
                break;

            case Troops.Infantry:
                knightSelected.Add(go);
                break;

            case Troops.King:
                King = go;
                break;
        }
        

        go.GetComponent<MovementManager>().selected = true;
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
                    go.GetComponent<MovementManager>().currentAction = new Normal(target, go);

                }
                foreach (GameObject go in this.archerSelected)
                {
                    go.GetComponent<MovementManager>().currentAction = new Normal(target, go);

                }
                if (King != null)
                {
                    King.GetComponent<MovementManager>().currentAction = new Normal(target, King);
                }
                break;

            case typeAction.Attack:
                foreach (GameObject go in this.knightSelected)
                {
                    go.GetComponent<MovementManager>().currentAction = new Attack(target, go);

                }
                foreach (GameObject go in this.archerSelected)
                {
                    go.GetComponent<MovementManager>().currentAction = new Attack(target, go);

                }
                if (King != null)
                {
                    King.GetComponent<MovementManager>().currentAction = new Attack(target, King);
                }
                break;

            case typeAction.Move:
                foreach (GameObject go in this.knightSelected)
                {
                    go.GetComponent<MovementManager>().currentAction = new Move(target, go);

                }
                foreach (GameObject go in this.archerSelected)
                {
                    go.GetComponent<MovementManager>().currentAction = new Move(target, go);

                }
                if (King != null)
                {
                    King.GetComponent<MovementManager>().currentAction = new Move(target, King);
                }
                break;

        }
    }

    public void makeAction(GameObject target, typeAction type)
        {
            switch (type)
            {
                case typeAction.Normal:
                    foreach (GameObject go in this.knightSelected)
                    {
                        go.GetComponent<MovementManager>().currentAction = new Normal(target, go);

                    }
                    foreach (GameObject go in this.archerSelected)
                    {
                        go.GetComponent<MovementManager>().currentAction = new Normal(target, go);

                    }
                    if (King != null)
                    {
                        King.GetComponent<MovementManager>().currentAction = new Normal(target, King);
                    }
                    break;

                case typeAction.Attack:
                    foreach (GameObject go in this.knightSelected)
                    {
                        go.GetComponent<MovementManager>().currentAction = new Attack(target, go);

                    }
                    foreach (GameObject go in this.archerSelected)
                    {
                        go.GetComponent<MovementManager>().currentAction = new Attack(target, go);

                    }
                    if (King != null)
                    {
                        King.GetComponent<MovementManager>().currentAction = new Attack(target, King);
                    }
                    break;

                case typeAction.Move:
                    foreach (GameObject go in this.knightSelected)
                    {
                        go.GetComponent<MovementManager>().currentAction = new Move(target, go);

                    }
                    foreach (GameObject go in this.archerSelected)
                    {
                        go.GetComponent<MovementManager>().currentAction = new Move(target, go);

                    }
                    if (King != null)
                    {
                        King.GetComponent<MovementManager>().currentAction = new Move(target, King);
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


}

