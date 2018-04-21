using Interfaces;
using UnityEngine;

namespace HitEffects
{
    public class DamageEffect : HitEffect
    {
        // Properties
        // -----------------------------------

        public float damage;
        
        // Constructors
        // -----------------------------------

        public DamageEffect(float damage = 10.0f)
        {
            this.damage = damage;
        }

        // Public methods
        // -----------------------------------

        public override void Apply(GameObject target)
        {
            IDamageable damageable = target.GetComponent<IDamageable>();

            if (damageable == null)
            {
                return;
            }
            
            damageable.ApplyDamage(damage);
        }
    }
}