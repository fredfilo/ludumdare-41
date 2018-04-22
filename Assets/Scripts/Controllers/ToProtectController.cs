using Interfaces;
using Notifications;
using UnityEngine;

namespace Controllers
{
    public class ToProtectController : MonoBehaviour, IDamageable
    {
        // Properties
        // ------------------------------------

        public float health = 100.0f;

        private float maxHealth;
    
        // Public methods
        // ------------------------------------
    
        public void ApplyDamage(float damage)
        {
            if (damage < 0)
            {
                return; // No healing this way.
            }

            health -= damage;

            Notification notification = new DefenseHealthNotification(health, maxHealth, "Tower", 1);
            GameController.instance.Broadcaster.Notify(notification);
            
            if (health <= 0)
            {
                notification = new DefenseDestroyedNotification("Tower", 1);
                GameController.instance.Broadcaster.Notify(notification);

                BoxCollider2D collider = GetComponent<BoxCollider2D>();
                Destroy(collider);
            }
        }
        
        // Private methods
        // ----------------------------------

        private void Start()
        {
            maxHealth = health;
        }
    }
}