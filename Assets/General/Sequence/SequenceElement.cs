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

    protected virtual void Awake()
    {
        startAt = Mathf.Infinity;
        state = State.waiting;
        hasCompletion = false;
    }

    public void Begin()
    {
        if(precedingDelay != 0)
        {
            Debug.Log("SequenceElement " + name + " delay started");
        }
        startAt = Time.time + precedingDelay;
    }

    protected virtual void DoBegin()
    {
        Debug.Log("SequenceElement " + name + " started");
        if(state != State.waiting) throw new Exception("DoBegin called twice");
        state = State.running;
        onStart.Invoke();
    }

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
                    Debug.Log("SequenceElement " + name + " finished");
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

    public void Complete()
    {
        Debug.Log(name + " got completion");
        hasCompletion = true;
    }

    public void SetTimeScale(float scale)
    {
        Time.timeScale = scale;
    }
}
