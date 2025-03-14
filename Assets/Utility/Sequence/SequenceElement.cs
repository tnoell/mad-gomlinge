using System;
using UnityEngine;
using UnityEngine.Events;

public class SequenceElement : MonoBehaviour
{
    [SerializeField] private float precedingDelay;
    public UnityEvent onStart;
    public UnityEvent onFinish;
    [SerializeField] private bool needsCompletion = false;

    private float startAt;
    private enum State
    {
        waiting,
        running,
        finished
    };
    [SerializeField] private State state; // serialized for debugging
    private bool hasCompletion;

    void Awake()
    {
        startAt = Mathf.Infinity;
        state = State.waiting;
        hasCompletion = false;
        SubAwake();
    }

    protected virtual void SubAwake() {}

    public void Begin()
    {
        if(precedingDelay == 0)
        {
            DoBegin();
        }
        else
        {
            Debug.Log("State " + name + " delay started");
            startAt = Time.time + precedingDelay;
        }
    }

    private void DoBegin()
    {
        Debug.Log("State " + name + " started");
        if(state != State.waiting) throw new Exception("DoBegin called twice");
        state = State.running;
        onStart.Invoke();
        SubBegin();
    }

    protected virtual void SubBegin() {}

    void Update()
    {
        switch(state)
        {
            case State.waiting:
                if (Time.time >= startAt) DoBegin();
                break;
            case State.running:
                SubUpdate();
                if(needsCompletion && !hasCompletion) break;
                if(SubIsFinished())
                {
                    onFinish.Invoke();
                    Debug.Log("State " + name + " finished");
                    state = State.finished;
                }
                break;
            default:
                break;
        }
    }

    protected virtual void SubUpdate() {}

    public bool IsFinished()
    {
        return state == State.finished;
    }

    protected virtual bool SubIsFinished() { return true; }

    public void Complete(bool complete)
    {
        Debug.Log(name + " got completion");
        hasCompletion = complete;
    }

    public void SetTimeScale(float scale)
    {
        Time.timeScale = scale;
    }
}
