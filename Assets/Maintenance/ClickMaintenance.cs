using UnityEngine;

public class ClickMaintenance : MaintenanceTask
{
    protected override void OnDoMaintenance()
    {
        Complete();
    }
}
