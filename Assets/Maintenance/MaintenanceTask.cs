using UnityEngine;

public abstract class MaintenanceTask : MonoBehaviour
{
    private MaintenanceTimer timer;
    
    void Start()
    {
        timer = GetComponent<MaintenanceTimer>();
        GetComponent<Module>().onDoMaintenance.AddListener(OnDoMaintenance);
    }

    protected void Complete()
    {
        timer.DoMaintenance();
    }

    protected abstract void OnDoMaintenance();
}
