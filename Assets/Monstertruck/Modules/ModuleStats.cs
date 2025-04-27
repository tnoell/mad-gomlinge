using UnityEngine;

public enum ModuleStat
{
    maintenanceReduction = 0
}

public class ModuleStats : ModifierManager<ModuleStat>
{
    MaintenanceTimer maintenanceTimer;

    protected override void Awake()
    {
        base.Awake();
        maintenanceTimer = GetComponent<MaintenanceTimer>();
    }

    override protected void Update()
    {
        base.Update();
        maintenanceTimer.SetDurationMultiplier(1 + GetValue(ModuleStat.maintenanceReduction));
    }
}
