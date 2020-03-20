using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpecialCommand
{
    public int Index { get; set; }

    public virtual void Execute()
    { }
}

