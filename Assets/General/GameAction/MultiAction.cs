using System;
using UnityEngine;

public class MultiAction : GameAction
{
    [SerializeReference] private GameAction[] actions;

    public override void Execute()
    {
        foreach (GameAction action in actions)
        {
            action.Execute();
        }
    }
}
