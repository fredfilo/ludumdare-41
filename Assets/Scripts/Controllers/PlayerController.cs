using UnityEngine;

namespace Controllers
{
    public class PlayerController : CharacterController
    {
        // Protected methods
        // -----------------------------------

        protected override float GetHorizontalMovement()
        {
            return Input.GetAxis("Horizontal");
        }

        protected override void ComputeVelocityForJump()
        {
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