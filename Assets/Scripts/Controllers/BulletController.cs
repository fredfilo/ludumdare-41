using Interfaces;
using Physics;
using UnityEngine;

namespace Controllers
{
    public class BulletController : MonoBehaviour
    {
        // Properties
        // --------------------------------------------

        public Vector2 direction;
        public float timeBeforeExplosion = 2.0f;
        public float speed = 1.0f;
        public float baseDamage = 60.0f;
        public float damageMultiplier = 1.0f;

        // Private methods
        // --------------------------------------------
        
        private void Update()
        {
            timeBeforeExplosion -= Time.deltaTime;
            if (timeBeforeExplosion <= 0)
            {
                Destroy(gameObject);
            }
            
            Vector3 position = transform.position;
            position.x += direction.x * speed * Time.deltaTime;
            position.y += direction.y * speed * Time.deltaTime;
            transform.position = position;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            IDamageable damageable = other.gameObject.GetComponent<IDamageable>();
            
            if (damageable != null)
            {
                damageable.ApplyDamage(baseDamage * damageMultiplier);
            }
            
            Destroy(gameObject);
        }
    }
}