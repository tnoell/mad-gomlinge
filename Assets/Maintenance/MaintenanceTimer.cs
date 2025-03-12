using System;
using UnityEngine;
using UnityEngine.Events;

public class MaintenanceTimer : MonoBehaviour
{
    public UnityEvent onExpired;
    public UnityEvent onRestored;
    [SerializeField] private float duration = 10f;

    private float progress;
    private bool isBroken;
    public bool IsBroken() { return isBroken; }
    private bool running;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        progress = 0;
        isBroken = false;
        running = true;
        Module module = GetComponent<Module>();
        if(module)
        {
            module.onAttached.AddListener(Restart);
            module.onUnattached.AddListener(Stop);
        }
    }

    public void Stop()
    {
        running = false;
    }

    public void Restart()
    {
        DoMaintenance();
        running = true;
    }

    public void Pause(bool pause)
    {
        running = !pause;
    }

    void FixedUpdate()
    {
        if(!running || isBroken) return;
        ChangeProgress(Time.fixedDeltaTime / duration);
    }

    public void ChangeProgress(float amount)
    {
        progress += amount;
        if(progress >= 1 && !isBroken)
        {
            isBroken = true;
            onExpired.Invoke();
        }
        progress = Mathf.Clamp(progress, 0, 1);
    }

    public void DoMaintenance()
    {
        if(isBroken)
        {
            onRestored.Invoke();
        }
        progress = 0;
        isBroken = false;
    }

    public float GetProgress() { return progress; }
}
