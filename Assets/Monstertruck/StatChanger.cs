using System;
using UnityEngine;

public class StatChanger : MonoBehaviour
{
    [SerializeField] float amount;
    [SerializeField] private PlayerStats.Stat stat;
    private PlayerStats.StatChange appliedStatChange;

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
        appliedStatChange = new PlayerStats.StatChange(stat, amount, gameObject);
        GameObject.FindWithTag("Player").GetComponent<PlayerStats>().AddChange(appliedStatChange);
    }

    public void Unapply()
    {
        if(appliedStatChange == null) return;
        appliedStatChange.Destroy();
        appliedStatChange = null;
    }
}
