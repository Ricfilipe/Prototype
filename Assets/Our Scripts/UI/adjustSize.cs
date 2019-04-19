using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class adjustSize : MonoBehaviour
{
    private RectTransform rect;
    // Start is called before the first frame update
    void Start()
    {
        rect = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        rect.localScale = new Vector3((Screen.width / (Screen.height + 0f) / (16 / (9+0f)) ), 1, 1);
    }
}
