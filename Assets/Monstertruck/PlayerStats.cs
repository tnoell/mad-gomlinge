using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private float baseSpeed = 0;

    public enum Stat
    {
        speed
    }

    public class StatChange
    {
        public Stat stat;
        public float amount;
        public GameObject source;
        public StatChange(Stat stat, float amount, GameObject source)
        {
            this.stat = stat;
            this.amount = amount;
            this.source = source;
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
                if(!statChanges[i].source)
                {
                    statChanges.RemoveAt(i);
                    continue;
                }
                currentValue += statChanges[i].amount;
            }
        }

        public float GetCurrent() { return currentValue; }
    }
    private ChangedStat speed;

    private ChangedStat[] allStats;

    void Awake()
    {
        speed = new ChangedStat(baseSpeed);

        allStats = new ChangedStat[]{ speed };
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
        case Stat.speed:
            return speed;
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
        return GetStat(Stat.speed);
    }

    public void AddChange(StatChange statChange)
    {
        GetChangedStat(statChange.stat).AddChange(statChange);
    }
}
