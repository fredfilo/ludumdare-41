using Physics;
using UnityEngine;

namespace Controllers
{
    public abstract class CharacterController : MovableObject
    {
        // Properties
        // -----------------------------------

        [Header("Movement")]
        
        [SerializeField] protected float movementSpeed = 1f;
        [SerializeField] protected float jumpSpeed = 7f;
        [SerializeField] protected float jumpSlowDown = 0.5f;

        protected SpriteRenderer renderer;
        
        // Protected methods
        // -----------------------------------

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

        protected abstract float GetHorizontalMovement();
        protected abstract void ComputeVelocityForJump();
 
        // Private methods
        // -----------------------------------
        
        private void Awake()
        {
            renderer = GetComponent<SpriteRenderer>();
        }
    }
}