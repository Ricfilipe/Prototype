using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveCamera:MonoBehaviour
{
    public Camera rtsCam;


    // Start is called before the first frame update
    public void moveCam(){
        // Debug.Log(Input.mousePosition.x-gameObject.transform.position.x );


        Vector3 helper = rtsCam.WorldToScreenPoint(gameObject.transform.position);

       float x = (Input.mousePosition.x -helper.x) / gameObject.GetComponent<RectTransform>().sizeDelta.x;
     
        float y = (Input.mousePosition.y - helper.z) / gameObject.GetComponent<RectTransform>().sizeDelta.y;
        Debug.Log(x + " " + y);
        rtsCam.transform.position=new Vector3(210-(420*y),80,(380*x)-190);
    }
}
