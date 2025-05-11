using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnityEventAction : GameAction
{
    public UnityEvent unityEvent;
    
    public override void Execute()
    {
        unityEvent.Invoke();
    }
}
