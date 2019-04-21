using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowModel : MonoBehaviour
{
    public GameObject unit;
   

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = unit.transform.position;
        transform.position = new Vector3(pos.x, pos.y + 3, pos.z);
    }
}
