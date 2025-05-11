using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetGameobjectActive : GameAction
{
    [SerializeField] private GameObject target;
    [SerializeField] private bool active;
    
    public override void Execute()
    {
        target.SetActive(active);
    }
}
