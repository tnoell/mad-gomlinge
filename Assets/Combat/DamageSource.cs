using System;
using UnityEngine;

namespace Combat
{
    public class DamageSource : MonoBehaviour
    {
        [SerializeField] private new string name;
        
        public string GetName() { return name; }

        private void Start()
        {
            if (name == "") Debug.LogError("DamageSource name missing on " + gameObject, gameObject);
        }
    }
}
