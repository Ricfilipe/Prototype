using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveCamera:MonoBehaviour
{
    public Camera rtsCam;
    public GameObject end;
    public GameObject start;

    // Start is called before the first frame update
    public void moveCam(){
        // Debug.Log(Input.mousePosition.x-gameObject.transform.position.x );


        Vector3 helper = start.transform.position;
        Vector3 helper2 = end.transform.position;

        Debug.Log(helper + " "+helper2);
      float x = (Input.mousePosition.x -helper.x) / (helper2.x-helper.x );
     
        float y = (Input.mousePosition.y - helper.y) / (helper2.y - helper.y+10);
      
        rtsCam.transform.position=new Vector3(210-(420*y),80,(380*x)-190);
    }
}
