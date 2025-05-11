using System;
using UnityEngine;

public class StatChanger : MonoBehaviour
{
    //TODO: change to OnEnable/OnDisable like in Trigger, so I can put multiple on a sub-object that I enable/disable
    [SerializeField] float amount;
    [SerializeField] private PlayerStat stat;
    private PlayerStats.Modifier appliedStatChange;

    void Awake()
    {
        appliedStatChange = null;
    }

    [ContextMenu("ToggleStat")]
    private void ToggleStat()
    {
        if(appliedStatChange == null)
            Apply();
        else
            Unapply();
    }

    public void Apply()
    {
        if(appliedStatChange != null) return;
        appliedStatChange = new PlayerStats.Modifier(stat, amount, gameObject);
        GameObject.FindWithTag("Player").GetComponent<PlayerStats>().AddModifier(appliedStatChange);
    }

    public void Unapply()
    {
        if(appliedStatChange == null) return;
        appliedStatChange.Destroy();
        appliedStatChange = null;
    }
}
