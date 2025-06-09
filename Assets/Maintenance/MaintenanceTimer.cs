using System;
using UnityEngine;
using UnityEngine.Events;

public class MaintenanceTimer : MonoBehaviour
{
    public UnityEvent onExpired;
    public UnityEvent onRestored;
    [SerializeField] private float duration = 10f;

    private ModuleGrid moduleGrid;

    private float progress;
    private bool isBroken;
    public bool IsBroken() { return isBroken; }
    private bool running;
    public bool IsRunning() { return running; }

    private float durationMultiplier;
    public void SetDurationMultiplier(float val) { durationMultiplier = val; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        moduleGrid = GameObject.FindWithTag("ModuleGrid").GetComponent<ModuleGrid>();
        progress = 0;
        isBroken = false;
        running = true;
        durationMultiplier = 1;
        Module module = GetComponent<Module>();
        if(module)
        {
            module.onAttached.AddListener(Restart);
            module.onUnattached.AddListener(Stop);
        }
    }

    private float GetModifiedDuration()
    {
        return duration * durationMultiplier;
    }

    public void Stop()
    {
        running = false;
    }

    public void Restart()
    {
        CompleteMaintenance();
        running = true;
    }

    public void Pause(bool pause)
    {
        running = !pause;
    }

    void FixedUpdate()
    {
        if(!running || isBroken) return;
        ChangeProgress(Time.fixedDeltaTime * moduleGrid.MaintenanceTimeScale / GetModifiedDuration());
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

    public void CompleteMaintenance()
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
