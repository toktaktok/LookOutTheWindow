using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class BaseNode : Node {

    public virtual void GetValue()
    {
        return;
    }
    public virtual string GetString()
    {
        return null;
    }

    public virtual bool IsBasicState()
    {
        return false;
    }


}