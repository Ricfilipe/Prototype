using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleClick : MonoBehaviour
{
    public  GameObject animation;
    public IEnumerator click(Vector3 point)
    {
        GameObject go=Instantiate(animation);
        go.transform.position = new Vector3(point.x, 0.25f, point.z);
        yield return new WaitForSeconds(2);
        Destroy(go);
    }
}
