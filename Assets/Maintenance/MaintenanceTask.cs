using UnityEngine;

public abstract class MaintenanceTask : MonoBehaviour
{
    [SerializeField] private EnableOverMaintenanceThreshold waitForMaintenanceThreshold;
    protected MaintenanceTimer maintenanceTimer;
    private bool needsMaintenance;
    
    protected virtual void Awake()
    {
        maintenanceTimer = GetComponent<MaintenanceTimer>();
        GetComponent<Module>().onDoMaintenance.AddListener(HandleMaintenance);
    }

    protected virtual void Start()
    {
        if(waitForMaintenanceThreshold)
        {
            needsMaintenance = false;
            waitForMaintenanceThreshold
                .onChange += (bool enable) => needsMaintenance = enable;
        }
        else
        {
            needsMaintenance = true;
        }
    }

    protected virtual void FixedUpdate() {}

    protected void Complete()
    {
        maintenanceTimer.CompleteMaintenance();
    }

    private void HandleMaintenance()
    {
        if(needsMaintenance) OnDoMaintenance();
    }

    protected abstract void OnDoMaintenance();
}
