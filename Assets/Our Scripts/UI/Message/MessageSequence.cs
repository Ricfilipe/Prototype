using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageSequence : Message
{
    public List<SingleMessage> list = new List<SingleMessage>();
    private int i = 0;

    public override bool advance()
    {
        i++;
        if (i < list.Count)
        {
            return true;
        }
        return false;
    }

    public override Sprite getImage()
    {
        return list[i].getImage();
    }

    public override string getMessage()
    {
        return list[i].getMessage();
    }

    public override string getName()
    {
        return list[i].getName();
    }
}
