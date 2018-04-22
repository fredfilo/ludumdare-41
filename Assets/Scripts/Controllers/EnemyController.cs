using System;
using Interfaces;
using UnityEngine;

namespace Controllers
{
    public class EnemyController : CharacterController, IFreezable
    {
        [SerializeField] public float direction = 1.0f;
        [SerializeField] public float freezeDuration = 0.0f;
        [SerializeField] public float damage = 50.0f;
        
        // Public methods
        // -----------------------------------
        
        /// <summary>
        /// IFreezable implementation.
        /// </summary>
        /// <param name="duration">The freeze duration</param>
        public void Freeze(float duration)
        {
            freezeDuration = duration;
        }

        // Protected methods
        // -----------------------------------

        protected override float GetHorizontalMovement()
        {
            if (freezeDuration > 0f)
            {
                freezeDuration -= Time.deltaTime;
                return 0;
            }
            
            return 1.0f * direction;
        }

        protected override void ComputeVelocityForJump()
        {
            // Enemies don't jump
        }

        protected override bool ShouldAllowCollision(RaycastHit2D hit, Vector2 normal)
        {
            GameObject otherGameObject = hit.collider.gameObject;
            
            if (otherGameObject.CompareTag("Enemy"))
            {
                return true;
            }

            if (otherGameObject.CompareTag("Projectile"))
            {
                return true;
            }

            IDamageable damageable = otherGameObject.GetComponent<IDamageable>();
            if (damageable != null)
            {
                freezeDuration = 2.0f;
                damageable.ApplyDamage(damage);
                animator.Play("Attack");
                return true;
            }
            
            if (Math.Abs(normal.y) < 0.00001f)
            {
                direction *= -1.0f;
            }

            return false;
        }

        protected override void Die()
        {
            Destroy(gameObject);
        }
    }
}