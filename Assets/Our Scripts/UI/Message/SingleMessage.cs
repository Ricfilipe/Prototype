using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleMessage : Message
{
    private string text;
    private Sprite image;
    private string name;

    public SingleMessage(string name,string text, string path)
    {
        this.text = text;
        this.name = name;

        if (path.Equals(""))
        {
            image = null;
        }
        else
        {
         
            image = Resources.Load<Sprite>(path);
            Debug.Log(image);
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
