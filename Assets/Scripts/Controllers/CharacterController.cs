using System;
using System.Collections.Generic;
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
        [SerializeField] protected float health = 100.0f;

        protected SpriteRenderer renderer;
        protected Animator animator;
        protected List<string> allowedCollisionTags = new List<string>();

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

        protected override void AfterMovement()
        {
            base.AfterMovement();
            CheckAnimations();
        }

        // Private methods
        // -----------------------------------
        
        private void Awake()
        {
            renderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
        }

        private void CheckAnimations()
        {
            if (animator == null)
            {
                return;
            }

            bool isMoving = (Math.Abs(velocity.x) > 0.000001f);
            bool isFalling = !grounded && velocity.y < 0;
            bool isJumping = !grounded && velocity.y > 0;

            animator.SetBool("isRunning", isMoving);
            animator.SetBool("isJumping", isJumping);
            animator.SetBool("isFalling", isFalling);
        }
    }
}