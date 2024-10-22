﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VoiceManager : MonoBehaviour
{

    private List<Message> queue = new List<Message>();
    private Message currentMessage = null;

    public GameObject Text;
    public GameObject Name;
    public GameObject Image;
    public GameObject Panel;
    private Text text;
    private Text textName;
    private Image image;

    private float temp=-1;

    // Start is called before the first frame update
    void Start()
    {
        Panel.active = false;
        text = Text.GetComponent<Text>();
        image = Image.GetComponent<Image>();
        textName = Name.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if(currentMessage == null)
        {
            if (queue.Count > 0)
            {
                currentMessage = queue[0];
                queue.RemoveAt(0);
                Panel.active = true;
                if (currentMessage.hasTimer)
                {
                    GameObject.FindGameObjectWithTag("Description").active = false;
                }
                else
                {
                    GameObject.FindGameObjectWithTag("Description").active = true ;
                }
            }
            else
            {
                Panel.active = false;
            }
        }
        else
        {
            if (currentMessage.hasTimer)
            {
                if(temp == -1)
                {
                    temp = currentMessage.time;
                }else if(temp > 0)
                {
                    temp -= Time.deltaTime;
                }
                else
                {
                    temp = -1;
                    if (!currentMessage.advance())
                    {
                        currentMessage = null;
                        return;
                    }
                }
                
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    if (!currentMessage.advance())
                    {
                        currentMessage = null;
                        return;
                    }
                }
            }
                textName.text = currentMessage.getName();
                text.text = currentMessage.getMessage();
                if (currentMessage.getImage() != null)
                {
                    image.sprite = currentMessage.getImage();
                }
            
        }
    }
    public void addToQueue(Message mes)
    {
        queue.Add(mes);
    }

    internal bool isEmpty()
    {
        return queue.Count == 0 && currentMessage == null;
    }
}
