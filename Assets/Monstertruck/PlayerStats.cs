using System;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerStat
{
    enginePower = 0
}

public class PlayerStats : ModifierManager<PlayerStat>
{
    [SerializeField] private float baseSpeed = 1;
    [SerializeField] private float speedPerEnginePower = 0.1f;

    public float GetSpeed()
    {
        float enginePower = GetValue(PlayerStat.enginePower);
        if (enginePower < 0.1) return 0;
        return baseSpeed + speedPerEnginePower * enginePower;
    }
}
