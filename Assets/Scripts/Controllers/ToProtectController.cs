using Interfaces;
using UnityEngine;

namespace Controllers
{
    public class ToProtectController : MonoBehaviour, IDamageable
    {
        // Properties
        // ------------------------------------

        public float health = 100.0f;
    
        // Public methods
        // ------------------------------------
    
        public void ApplyDamage(float damage)
        {
            if (damage < 0)
            {
                return; // No healing this way.
            }

            health -= damage;

            if (health <= 0)
            {
                // GameOver
            }
        }
    }
}