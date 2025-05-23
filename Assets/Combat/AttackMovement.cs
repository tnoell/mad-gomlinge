using UnityEngine;

namespace Combat
{
    public abstract class AttackMovement : MonoBehaviour
    {
        [SerializeField] protected Attack attack;
        protected Combatant target;
        protected DamageSource damageSource;

        public void Launch(DamageSource source, Combatant target)
        {
            this.target = target;
            this.damageSource = source;
            gameObject.SetActive(true);
            if(!target) return;
            LaunchImpl();
        }

        protected abstract void LaunchImpl();
    }
}
