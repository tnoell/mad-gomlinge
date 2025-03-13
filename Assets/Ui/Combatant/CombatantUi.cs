using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ui
{
    public class CombatantUi : MonoBehaviour
    {
        [SerializeField] private HealthBar healthBar;
    
        public void AttachTo(Combat.Combatant combatant)
        {
            GetComponent<UiTrackObject>().Track(combatant.gameObject);
            healthBar.SetCombatant(combatant);
        }
    }
}