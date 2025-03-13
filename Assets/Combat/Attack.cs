using UnityEngine;
using UnityEngine.Events;

namespace Combat
{
    public class Attack : MonoBehaviour
    {
        public UnityEvent onHit;
        [SerializeField] private float damageAmount;

        public void Hit(DamageSource source, Combatant target)
        {
            onHit.Invoke();
            target.Hit(damageAmount, source);
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}
