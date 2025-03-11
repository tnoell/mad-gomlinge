using UnityEngine;
using UnityEngine.Events;

public class AnimationTriggeredEvent : MonoBehaviour
{
    public UnityEvent events;
    public void Trigger()
    {
        events.Invoke();
    }
}
