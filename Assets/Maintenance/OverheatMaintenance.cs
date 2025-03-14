using System;
using UnityEngine;

public class OverheatMaintenance : MaintenanceTask
{
    [SerializeField] private Behaviour[] activatableComponents;
    [SerializeField] private float fullCooldownSeconds = 10;
    [SerializeField] private AnimatorSharedVariable<bool> moduleActivated;
    
    protected override void Awake()
    {
        base.Awake();
        moduleActivated.Val = true;
        maintenanceTimer.onExpired.AddListener(() => ActivateModule(false));
    }

    protected override void OnDoMaintenance()
    {
        if(maintenanceTimer.IsBroken())
        {
            if(maintenanceTimer.GetProgress() > 0) return;
            Complete();
        }
        ActivateModule(!moduleActivated.Val);
    }

    private void ActivateModule(bool activate)
    {
        moduleActivated.Val = activate;
        maintenanceTimer.Pause(!activate);
        foreach(Behaviour component in activatableComponents)
        {
            component.enabled = moduleActivated.Val;
        }
    }

    protected override void FixedUpdate()
    {
        if(!moduleActivated.Val)
        {
            maintenanceTimer.ChangeProgress(-Time.fixedDeltaTime / fullCooldownSeconds);
        }
    }
}
