using UnityEngine;

namespace Combat
{
    public class Attack : MonoBehaviour
    {
        [SerializeField] private float damageAmount;

        public void Hit(DamageSource source, Combatant target)
        {
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
