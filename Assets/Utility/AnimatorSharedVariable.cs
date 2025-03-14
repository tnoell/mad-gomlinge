using System;
using UnityEngine;

[Serializable]
public class AnimatorSharedVariable<T>
{
    [SerializeField] private Animator sharedWithAnimator;
    [SerializeField] private string variableName;
    private T val;
    public T Val
    {
        get { return val; }
        set {
            val = value;
            switch(val)
            {
            case bool b:
                sharedWithAnimator.SetBool(variableName, b);
                break;
            default:
                throw new NotImplementedException();
            }
        }
    }
}