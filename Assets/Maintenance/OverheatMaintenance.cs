using UnityEngine;

public class OverheatMaintenance : MaintenanceTask
{
    [SerializeField] private Behaviour[] activatableComponents;
    [SerializeField] private float fullCooldownSeconds = 10;
    private bool moduleActivated;
    
    protected override void Awake()
    {
        base.Awake();
        moduleActivated = true;
        maintenanceTimer.onExpired.AddListener(() => ActivateModule(false));
    }

    protected override void OnDoMaintenance()
    {
        if(maintenanceTimer.IsBroken())
        {
            if(maintenanceTimer.GetProgress() > 0) return;
            Complete();
        }
        ActivateModule(!moduleActivated);
    }

    private void ActivateModule(bool activate)
    {
        moduleActivated = activate;
        maintenanceTimer.Pause(!activate);
        foreach(Behaviour component in activatableComponents)
        {
            component.enabled = moduleActivated;
        }
    }

    protected override void FixedUpdate()
    {
        if(!moduleActivated)
        {
            maintenanceTimer.ChangeProgress(-Time.fixedDeltaTime / fullCooldownSeconds);
        }
    }
}
