using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public List<GameObject> myCharacterPool;
    public GameObject selectionMenu;
    [HideInInspector]
    public List<GameObject> selectedCharacter;

    [SerializeField]
    private RectTransform selectSquareImage;

    bool activateSelectArea = false;
    bool UIclick = false;
    Vector3 startPos, endPos;

    private void Awake()
    {
        selectedCharacter = new List<GameObject>();
        myCharacterPool = new List<GameObject>();
    }

    // Start is called before the first frame update
    void Start()
    {
        selectSquareImage.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
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
                        clearSelection();
                        addToSelection(hit);
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
                            clearSelection();
                            count++;
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


        if (Input.GetMouseButtonDown(1) && selectedCharacter.Count > 0)
        {

            RaycastHit hit;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 300))
            {
                if (hit.collider.tag == "Enemie")
                {
                    foreach (GameObject go in this.selectedCharacter) {
                        go.GetComponent<MovementManager>().currentAction = new Normal(hit.collider.gameObject,go);

                    }
                }
                else
                {
                    foreach (GameObject go in this.selectedCharacter)
                    {
                        go.GetComponent<MovementManager>().currentAction = new Normal(hit.point,go);

                    }

                }

            }
        }




     }

    public void clearSelection()
    {

        foreach (GameObject go in selectedCharacter)
        {
            go.GetComponent<MovementManager>().selected = false;
        }
        selectedCharacter.Clear();
        selectionMenu.GetComponent<generateSelection>().change = true;
    }

    public void addToSelection( RaycastHit hit) {
        hit.collider.GetComponent<MovementManager>().selected = true;
        selectedCharacter.Add(hit.collider.gameObject);
    }

    public void addToSelection(GameObject go)
    {
        selectedCharacter.Add(go);

        go.GetComponent<MovementManager>().selected = true;
    }
}

