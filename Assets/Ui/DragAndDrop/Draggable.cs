using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine.Serialization;

public class Draggable : MonoBehaviour
{
    [FormerlySerializedAs("onDragReceivedUnity")]
    public UnityEvent onDragReceivedUnity;
    public UnityEvent onDragStart;
    public UnityEvent onDragEnd;

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

    private void SetBeingDragged(bool val)
    {
        if(isBeingDragged == val) return;
        isBeingDragged = val;
        if(isBeingDragged)
        {
            onDragStart.Invoke();
        }
        else 
        {
            onDragEnd.Invoke();
        }
    }

    void OnMouseDrag()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;
        transform.position = pos;
        SetBeingDragged(true);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger enter: " + this.name + " - " + other.name);
        overlapping.Add(other);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("Trigger exit: " + this.name + " - " + other.name);
        overlapping.Remove(other);
    }

    void OnMouseUp()
    {
        if(!isBeingDragged) return;
        bool received = false;
        foreach(Collider2D other in overlapping)
        {
            DragReceiver receiver = other.GetComponent<DragReceiver>();
            if(receiver && receiver.Receive(this))
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
        SetBeingDragged(false);
    }
}
