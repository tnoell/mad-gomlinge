using System;
using UnityEngine;

public class StatChanger : MonoBehaviour
{
    [SerializeField] float amount;
    [SerializeField] private PlayerStats.Stat stat;
    private bool applied;

    void Awake()
    {
        applied = false;
    }

    public void OnAttachChanged()
    {
        Module module = GetComponent<Module>();
        if(module.IsAttached())
        {
            Apply();
        }
    }

    public void Apply()
    {
        if(applied) throw new Exception("Already applied");
        PlayerStats.StatChange change = new PlayerStats.StatChange(stat, amount, gameObject);
        GameObject.FindWithTag("Player").GetComponent<PlayerStats>().AddChange(change);
        applied = true;
    }
}
