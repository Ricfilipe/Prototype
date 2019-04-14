using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelInterface:MonoBehaviour
{
    public GameObject potrait;
    public GameObject HPBar;

    private MenuHPBar hp;
    [HideInInspector]
    public GameObject selection;

    private void Awake()
    {
        this.hp = HPBar.GetComponent<MenuHPBar>();
        this.GetComponent<Button>().onClick.AddListener(click);
    }

    private void click() {
        selection.GetComponent<generateSelection>().changeSelection(gameObject);
    }

    public void updateHPbar(GameObject unit)
    {
        hp.updateBar(unit);
    }
}
