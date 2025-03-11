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

    public void OnAttachChanged()
    {
        Module module = GetComponent<Module>();
        if(module.IsAttached())
        {
            Apply();
        }
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
        if(appliedStatChange != null) throw new Exception("Already applied");
        appliedStatChange = new PlayerStats.StatChange(stat, amount, gameObject);
        GameObject.FindWithTag("Player").GetComponent<PlayerStats>().AddChange(appliedStatChange);
    }

    public void Unapply()
    {
        appliedStatChange.Destroy();
        appliedStatChange = null;
    }
}
