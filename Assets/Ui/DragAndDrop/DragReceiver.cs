using UnityEngine;
using UnityEngine.Events;

public class DragReceiver : MonoBehaviour
{
    public UnityEvent onReceive;
    [SerializeField] private string mustHaveComponent;
    [SerializeField] private string mustHaveDragTag;
    
    public bool Receive(Draggable draggable)
    {
        if(!CanReceive(draggable)) return false;
        onReceive.Invoke();
        return true;
    }

    public bool CanReceive(Draggable draggable)
    {
        if (mustHaveComponent != ""
        && draggable.GetComponent(mustHaveComponent) == null)
        {
            return false;
        }
        if (mustHaveDragTag != ""
        && draggable.GetComponent<DragTag>()?.tag != mustHaveDragTag)
        {
            return false;
        }
        return true;
    }
}
