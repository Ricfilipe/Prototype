using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class  Message 
{
    // Start is called before the first frame update
    public abstract string getMessage();

    public abstract string getName();

    public abstract Sprite getImage();

    public abstract bool advance();

}
