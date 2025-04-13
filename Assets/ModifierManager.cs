using System;
using System.Collections.Generic;
using UnityEngine;

public class ModifierManager<IdentifierEnum> : MonoBehaviour where IdentifierEnum : System.Enum
{
    public class Modifier
    {
        private IdentifierEnum identifier;
        private float amount;
        private GameObject source;

        public Modifier(IdentifierEnum identifier, float amount, GameObject source)
        {
            this.identifier = identifier;
            this.amount = amount;
            this.source = source;
        }

        public float Get() { return amount; }
        public IdentifierEnum GetIdentifier() { return identifier; }
        public bool IsValid() { return source; }

        public void Destroy()
        {
            source = null;
            amount = 0;
        }
    }

    private class ModifiableValue
    {
        private List< Modifier > modifiers;
        private float currentValue;

        public ModifiableValue() {
            modifiers = new List< Modifier >();
            currentValue = 0;
        }

        public void AddModifier(Modifier modifier)
        {
            modifiers.Add(modifier);
        }
        
        public void Update()
        {
            currentValue = 0;
            for(int i = modifiers.Count - 1; i >= 0; i--)
            {
                if(!modifiers[i].IsValid())
                {
                    modifiers.RemoveAt(i);
                    continue;
                }
                currentValue += modifiers[i].Get();
            }
        }

        public float GetCurrent() { return currentValue; }
    }

    private ModifiableValue[] modifiableValues;

    void Awake()
    {
        modifiableValues = Util.DefaultedArray<ModifiableValue>(Enum.GetNames(typeof(IdentifierEnum)).Length);
    }

    void Update()
    {
        foreach(ModifiableValue value in modifiableValues)
        {
            value.Update();
        }
    }

    private ModifiableValue GetModifiableValue(IdentifierEnum identifier)
    {
        return modifiableValues[Convert.ToInt32(identifier)];
    }

    protected float GetValue(IdentifierEnum identifier)
    {
        return GetModifiableValue(identifier).GetCurrent();
    }

    public void AddModifier(Modifier modifier)
    {
        GetModifiableValue(modifier.GetIdentifier()).AddModifier(modifier);
    }
}