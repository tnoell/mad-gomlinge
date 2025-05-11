using System;
using UnityEngine;

// used by animation events, which can't distinguish between
// multiple components of the same type (which is how I
// would've liked to handle multiple animation events)
public class ActionSequence : GameAction
{
    [SerializeReference] private GameAction[] actions;

    private int counter = 0;

    public override void Execute()
    {
        actions[counter].Execute();
        counter++;
        if (counter >= actions.Length)
        {
            counter = 0;
        }
    }
}
