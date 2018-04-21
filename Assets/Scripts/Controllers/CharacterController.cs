using Interfaces;
using Physics;
using UnityEngine;

namespace Controllers
{
    public abstract class CharacterController : MovableObject, IDamageable
    {
        // Properties
        // -----------------------------------

        [Header("Movement")]
        
        [SerializeField] protected float movementSpeed = 1f;
        [SerializeField] protected float jumpSpeed = 7f;
        [SerializeField] protected float jumpSlowDown = 0.5f;

        protected SpriteRenderer renderer;
        protected float health = 100.0f;

        // Public methods
        // -----------------------------------
        
        /// <summary>
        /// IDamageable implementation.
        /// </summary>
        /// <param name="damage">The damage taken.</param>
        public virtual void ApplyDamage(float damage)
        {
            if (damage < 0)
            {
                return; // We don't heal by applying damage.
            }
            
            health -= damage;

            if (health <= 0)
            {
                Die();
            }
        }
        
        // Protected methods
        // -----------------------------------

        protected abstract float GetHorizontalMovement();
        protected abstract void ComputeVelocityForJump();
        
        protected override void ComputeVelocity()
        {
            base.ComputeVelocity();
            
            ComputeVelocityForJump();
            
            targetVelocity = new Vector2(GetHorizontalMovement() * movementSpeed, 0);
            
            // Check if the sprite must be flipped horizontally.
            bool mustFlip = renderer.flipX ? (targetVelocity.x > 0) : (targetVelocity.x < 0);
            if (mustFlip)
            {
                renderer.flipX = !renderer.flipX;
            }
        }

        protected virtual void Die()
        {
            
        }
 
        // Private methods
        // -----------------------------------
        
        private void Awake()
        {
            renderer = GetComponent<SpriteRenderer>();
        }
    }
}