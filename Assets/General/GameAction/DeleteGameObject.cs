using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteGameObject : GameAction
{
    [SerializeField] private GameObject target;
    
    public override void Execute()
    {
        GameObject.Destroy(target);
    }
}
