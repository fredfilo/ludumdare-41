using System;
using Physics;
using UnityEngine;

namespace Controllers
{
    public class PlayerController : MovableObject
    {
        // Properties
        // -----------------------------------

        [Header("Movement")]
        
        [SerializeField] private float movementSpeed = 1f;
        [SerializeField] private float jumpSpeed = 7f;
        [SerializeField] private float jumpSlowDown = 0.5f;

        private SpriteRenderer renderer;
        
        // Protected methods
        // -----------------------------------

        protected override void ComputeVelocity()
        {
            base.ComputeVelocity();
            
            // Modify velocity for jump.
            if (grounded && Input.GetButtonDown("Jump"))
            {
                velocity.y = jumpSpeed;
            }
            else if (Input.GetButtonUp("Jump"))
            {
                if (velocity.y > 0)
                {
                    velocity.y *= jumpSlowDown;
                }
            }
            
            targetVelocity = new Vector2(Input.GetAxis("Horizontal") * movementSpeed, 0);
            
            // Check if the sprite must be flipped horizontally.
            bool mustFlip = renderer.flipX ? (targetVelocity.x > 0) : (targetVelocity.x < 0);
            if (mustFlip)
            {
                renderer.flipX = !renderer.flipX;
            }
        }
 
        // Private methods
        // -----------------------------------
        
        private void Awake()
        {
            renderer = GetComponent<SpriteRenderer>();
        }
    }
}