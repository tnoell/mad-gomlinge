using UnityEngine;
using UnityEngine.Events;

public class MaintenanceTimer : MonoBehaviour
{
    public UnityEvent onExpired;
    public UnityEvent onRestored;
    [SerializeField] private float duration = 10f;

    private float progress;
    private bool isBroken;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        progress = 0;
        isBroken = false;
    }

    void FixedUpdate()
    {
        if(isBroken) return;
        progress += Time.fixedDeltaTime / duration;
        if(progress >= 1)
        {
            isBroken = true;
            onExpired.Invoke();
        }
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
