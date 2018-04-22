using System;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;

namespace Controllers
{
    public class EnemyController : CharacterController, IFreezable
    {
        [SerializeField] public float direction = 1.0f;
        [SerializeField] public float freezeDuration = 0.0f;
        [SerializeField] public float damage = 50.0f;

        private bool isAttacking;
        
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

        protected override void Start()
        {
            base.Start();

            isEnemy = true;
            
            allowedCollisionTags = new List<string>()
            {
                "Player",
                "Enemy",
                "Projectile",
                "ToProtect"
            };
        }

        protected override float GetHorizontalMovement()
        {
            if (IsFrozen())
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

        protected override void ComputeVelocity()
        {
            base.ComputeVelocity();

            if (velocity.y > 0)
            {
                velocity.y = 0;
            }
        }

        protected override bool ShouldAllowCollision(RaycastHit2D hit, Vector2 normal, Vector2 movement)
        {
            GameObject otherGameObject = hit.collider.gameObject;

            if (otherGameObject.CompareTag("Enemy"))
            {
                return true;
            }
            
            isAttacking = false;
            
            IDamageable damageable = otherGameObject.GetComponent<IDamageable>();
            if (damageable != null && CanAttack())
            {
                freezeDuration = 0.75f;
                damageable.ApplyDamage(damage);
                animator.Play("Attack");
                isAttacking = true;
                return true;
            }
            
            if (allowedCollisionTags.Contains(otherGameObject.tag))
            {
                return true;
            }
            
            if (!isAttacking && Math.Abs(normal.y) < 0.00001f)
            {
                direction *= -1.0f;
            }

            return false;
        }

        protected override void Die()
        {
            Destroy(gameObject);
        }

        protected bool IsFrozen()
        {
            return freezeDuration > 0f;
        }

        protected bool CanAttack()
        {
            return !IsFrozen();
        }
    }
}