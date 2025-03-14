using UnityEngine;
namespace VisualEffects
{
    public class ScreenShakeSource : MonoBehaviour
    {
        [SerializeField] private ScreenShakeAction screenShake;
        [SerializeField] private Combat.Combatant listenForDamage;
        [SerializeField] private AnimationCurve multiplierPerDamage;

        void Start()
        {
            if(listenForDamage)
            {
                listenForDamage.onHealthChanged += ApplyByHealthChange;
            }
        }

        public void ApplyByHealthChange(float healthChange)
        {
            if(healthChange >= 0) return;
            float multiplier = multiplierPerDamage.Evaluate(-healthChange);
            Apply(multiplier);
        }

        [ContextMenu("Apply")]
        public void Apply(float multiplier = 1)
        {
            screenShake.Apply(multiplier);
        }
    }
}
