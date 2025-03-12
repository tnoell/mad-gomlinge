using UnityEngine;
using UnityEngine.Events;

public abstract class Minigame : MonoBehaviour
{
    public UnityEvent onCompleted;

    protected void Complete()
    {
        onCompleted.Invoke();
        GameObject.Destroy(gameObject);
    }
}
