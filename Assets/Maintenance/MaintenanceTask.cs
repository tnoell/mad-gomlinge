using UnityEngine;

public abstract class MaintenanceTask : MonoBehaviour
{
    protected MaintenanceTimer maintenanceTimer;
    
    protected virtual void Awake()
    {
        maintenanceTimer = GetComponent<MaintenanceTimer>();
        GetComponent<Module>().onDoMaintenance.AddListener(OnDoMaintenance);
    }

    protected virtual void FixedUpdate() {}

    protected void Complete()
    {
        maintenanceTimer.DoMaintenance();
    }

    protected abstract void OnDoMaintenance();
}
