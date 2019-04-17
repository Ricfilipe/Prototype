using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveCamera:MonoBehaviour
{
    public Camera rtsCam;


    // Start is called before the first frame update
    public void moveCam(){
        // Debug.Log(Input.mousePosition.x-gameObject.transform.position.x );


        Vector3 helper = gameObject.transform.position;

      float x = (Input.mousePosition.x -helper.x) / (gameObject.GetComponent<RectTransform>().sizeDelta.x  *(1920 / Screen.width));
     
        float y = (Input.mousePosition.y - helper.y) / (gameObject.GetComponent<RectTransform>().sizeDelta.y*(1080/Screen.height));
      
        rtsCam.transform.position=new Vector3(210-(420*y),80,(380*x)-190);
    }
}
