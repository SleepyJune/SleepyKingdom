using System;
using System.Collections;
using System.Collections.Generic;

public class SceneTarget
{
    public Type type;
    public object obj;

    public SceneTarget(Type type, object obj)
    {
        this.type = type;
        this.obj = obj;
    }
}