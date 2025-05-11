using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// an Action executed through external means, such as an animation event
public class ActionOnExternal : MonoBehaviour
{
    [SerializeReference] private GameAction action;

    [ContextMenu("Execute")]
    public void Execute()
    {
        action.Execute();
    }
}
