using System;
using UnityEngine;

namespace Controllers
{
    public class EnemyController : CharacterController
    {
        [SerializeField] public float direction = 1.0f;
        
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

        protected override bool ShouldAllowCollision(RaycastHit2D hit, Vector2 normal)
        {
            if (hit.collider.gameObject.CompareTag("Enemy"))
            {
                return true;
            }
            
            if (Math.Abs(normal.y) < 0.00001f)
            {
                direction *= -1.0f;
            }

            return false;
        }
    }
}