using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedMessage : Message
{

    private string text;
    private Sprite image;
    private string name;
    public TimedMessage(int time, string name, string text, string path)
    {
        this.time = time;
        this.hasTimer = true;
        this.text = text;
        this.name = name;
        if (path.Equals(""))
        {
            image = null;
        }
        else
        {
            image = Resources.Load<Sprite>(path);
        }
    }
    public override bool advance()
    {
        return false;
    }

    public override Sprite getImage()
    {
        return image;
    }

    public override string getMessage()
    {
        return text;
    }

    public override string getName()
    {
        return name;
    }

}
