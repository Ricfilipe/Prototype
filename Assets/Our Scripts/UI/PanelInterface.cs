using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelInterface:MonoBehaviour
{
    public GameObject potrait;
    public GameObject HPBar,progressionActive, progressionCD;

    private MenuHPBar hp;
    [HideInInspector]
    public GameObject selection;

    private void Awake()
    {
        this.hp = HPBar.GetComponent<MenuHPBar>();
        this.GetComponent<Button>().onClick.AddListener(click);
        
    }

    public void UpdateProgressionBar(MovementManager move)
    {
        if (move.abs.Count > 0)
        {
            if (move.abs[0].hasCd)
            {
                if (move.abs[0].state == Ability.State.Available)
                {
                    progressionCD.GetComponent<Image>().fillAmount = 1f;
                    progressionActive.active = false;
                    progressionCD.active = true;
                    progressionCD.GetComponent<Image>().color = Color.green;
                }
                else if (move.abs[0].state == Ability.State.OnCooldown)
                {
                    progressionActive.active = false;
                    progressionCD.active = true;
                    progressionCD.GetComponent<Image>().fillAmount = (Time.time - move.abs[0].timeOnCooldown) / move.abs[0].cooldown+0f;
                    progressionCD.GetComponent<Image>().color = new Color(0.1631363f, 2852166f, 0.735849f);
                }
            }
            else
            {
                progressionCD.active = false;
            }

            if (move.abs[0].hasActivation)
            {
                if (move.abs[0].state == Ability.State.Active)
                {
                    progressionActive.GetComponent<Image>().fillAmount = 1 - Mathf.Min(1, (Time.time - move.abs[0].timeOnActivation) / move.abs[0].activeTime + 0f);
                    progressionCD.active = false;
                    progressionActive.active = true;
                }

            }
            else
            {
                progressionActive.active = false;
            }
        }
    }


    private void click() {
        selection.GetComponent<generateSelection>().changeSelection(gameObject);
    }

    public void updateHPbar(GameObject unit)
    {
        hp.updateBar(unit);
    }

    public void putImage(Sprite image)
    {
        potrait.GetComponent<Image>().sprite = image;
    }


}
