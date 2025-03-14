using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class Draggable : MonoBehaviour
{
    public UnityEvent onDragReceivedUnity;

    public delegate void OnDragFinished(bool received);
    public OnDragFinished onDragFinished;
    private bool isBeingDragged;
    private HashSet<Collider2D> overlapping;

    void Awake()
    {
        onDragFinished = null;
        isBeingDragged = false;
        overlapping = new HashSet<Collider2D>();
    }

    void OnMouseDrag()
    {
        isBeingDragged = true;
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;
        transform.position = pos;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        overlapping.Add(other);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        overlapping.Remove(other);
    }

    void OnMouseUp()
    {
        if(!isBeingDragged) return;
        isBeingDragged = false;
        bool received = false;
        foreach(Collider2D other in overlapping)
        {
            DragReceiver receiver = other.GetComponent<DragReceiver>();
            if(receiver.Receive(this))
            {
                received = true;
                break;
            }
        }

        onDragFinished?.Invoke(received);
        if(received)
        {
            onDragReceivedUnity.Invoke();
        }
    }
}
