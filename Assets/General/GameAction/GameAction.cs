using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class GameAction : DrawableAbstractClass
{
    public abstract void Execute();
}
