using System;
using UnityEngine;

namespace Controllers
{
    public class EnemyController : CharacterController
    {
        [SerializeField] private float direction = 1.0f;
        
        // Protected methods
        // -----------------------------------

        protected override float GetHorizontalMovement()
        {
            return 1.0f * direction;
        }

        protected override void ComputeVelocityForJump()
        {
            // Enemies don't jump
        }

        protected override void OnCollision(Vector2 normal)
        {
            if (Math.Abs(normal.y) < 0.00001f)
            {
                direction *= -1.0f;
            }
        }
    }
}