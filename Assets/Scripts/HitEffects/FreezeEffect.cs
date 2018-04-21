using Interfaces;
using UnityEngine;

namespace HitEffects
{
    public class FreezeEffect : HitEffect
    {
        // Properties
        // -----------------------------------
        
        public float duration;
        
        // Constructors
        // -----------------------------------

        public FreezeEffect(float duration = 1.0f)
        {
            this.duration = duration;
        }

        // Public methods
        // -----------------------------------
        
        public override void Apply(GameObject target)
        {
            IFreezable freezable = target.GetComponent<IFreezable>();

            if (freezable == null)
            {
                return;
            }
            
            freezable.Freeze(duration);
        }
    }
}