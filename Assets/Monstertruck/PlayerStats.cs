using System;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerStat
{
    enginePower = 0,
    defense
}

public class PlayerStats : ModifierManager<PlayerStat>
{
    [SerializeField] private float baseSpeed = 1;
    [SerializeField] private float speedPerEnginePower = 0.1f;
    [SerializeField] private float damageMultiplierPerDefense = 0.9f;
    Combat.Combatant combatant;

    protected override void Awake()
    {
        base.Awake();
        combatant = GetComponent<Combat.Combatant>();
    }

    override protected void Update()
    {
        base.Update();
        combatant.SetDamageMultiplier(GetDamageMultiplier());
    }

    public float GetSpeed()
    {
        float enginePower = GetValue(PlayerStat.enginePower);
        if (enginePower < 0.1) return 0;
        return baseSpeed + speedPerEnginePower * enginePower;
    }

    public float GetDamageMultiplier()
    {
        return Mathf.Pow(damageMultiplierPerDefense, GetValue(PlayerStat.defense));
    }
}
