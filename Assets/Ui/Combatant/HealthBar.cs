using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Ui
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Combat.Combatant combatant;
        private Slider slider;

        public void SetCombatant(Combat.Combatant combatant)
        {
            this.combatant = combatant;
        }

        void Awake()
        {
            if(combatant == null)
                combatant = GameObject.FindWithTag("Player").GetComponent<Combat.Combatant>();
        }

        // Start is called before the first frame update
        void Start()
        {
            slider = GetComponent<Slider>();
        }

        // Update is called once per frame
        void Update()
        {
            slider.value = combatant.GetHealthFraction();
        }
    }
}
