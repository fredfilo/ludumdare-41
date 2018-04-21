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

        protected override void Start()
        {
            base.Start();

            renderer = GetComponent<SpriteRenderer>();
        }

        protected override void ComputeVelocity()
        {
            base.ComputeVelocity();

            targetVelocity = new Vector2(Input.GetAxis("Horizontal") * movementSpeed, 0);
            
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
        }
    }
}