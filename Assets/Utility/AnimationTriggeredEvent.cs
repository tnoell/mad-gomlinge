using UnityEngine;
using UnityEngine.Events;

public class AnimationTriggeredEvent : MonoBehaviour
{
    public UnityEvent events;
    public void Trigger()
    {
        events.Invoke();
    }

    public void Destroy(GameObject obj)
    {
        GameObject.Destroy(obj);
    }

    public void DestroyThis(float delay)
    {
        GameObject.Destroy(gameObject, delay);
    }
}
