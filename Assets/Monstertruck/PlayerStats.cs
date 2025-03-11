using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private float baseSpeed = 1;
    [SerializeField] private float speedPerEnginePower = 0.1f;

    public enum Stat
    {
        enginePower
    }

    public class StatChange
    {
        private Stat stat;
        private float amount;
        private GameObject source;

        public StatChange(Stat stat, float amount, GameObject source)
        {
            this.stat = stat;
            this.amount = amount;
            this.source = source;
        }

        public float Get() { return amount; }
        public Stat GetStat() { return stat; }
        public bool IsValid() { return source; }

        public void Destroy()
        {
            source = null;
            amount = 0;
        }
    }

    private class ChangedStat
    {
        private float baseValue;
        private List<StatChange> statChanges;
        private float currentValue;
        public ChangedStat(float baseValue) {
            this.baseValue = baseValue;
            statChanges = new List<StatChange>();
            currentValue = baseValue;
        }

        public void AddChange(StatChange statChange)
        {
            statChanges.Add(statChange);
        }
        
        public void Update()
        {
            currentValue = baseValue;
            for(int i = statChanges.Count - 1; i >= 0; i--)
            {
                if(!statChanges[i].IsValid())
                {
                    statChanges.RemoveAt(i);
                    continue;
                }
                currentValue += statChanges[i].Get();
            }
        }

        public float GetCurrent() { return currentValue; }
    }
    private ChangedStat enginePower;

    private ChangedStat[] allStats;

    void Awake()
    {
        enginePower = new ChangedStat(0);

        allStats = new ChangedStat[]{ enginePower };
    }

    void Update()
    {
        foreach(ChangedStat stat in allStats)
        {
            stat.Update();
        }
    }

    private ChangedStat GetChangedStat(Stat stat)
    {
        switch(stat)
        {
        case Stat.enginePower:
            return enginePower;
        default:
            throw new Exception("unhandled stat: " + stat);
        }
    }

    public float GetStat(Stat stat)
    {
        return GetChangedStat(stat).GetCurrent();
    }

    public float GetSpeed()
    {
        float enginePower = GetStat(Stat.enginePower);
        if (enginePower < 0.1) return 0;
        return baseSpeed + speedPerEnginePower * enginePower;
    }

    public void AddChange(StatChange statChange)
    {
        GetChangedStat(statChange.GetStat()).AddChange(statChange);
    }
}
