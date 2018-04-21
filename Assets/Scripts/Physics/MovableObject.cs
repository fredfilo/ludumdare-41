using System.Collections.Generic;
using UnityEngine;

namespace Physics
{
    public class MovableObject : MonoBehaviour
    {
        // Properties
        // -----------------------------------

        [Header("Physics")]
        
        public float gravityModifier = 1.0f;
        public float minimumNormalY = 0.65f;
        
        protected Rigidbody2D rigidBody;
        protected const float minimumMovementDistance = 0.001f;
        protected const float shellRadius = 0.01f;
        
        protected Vector2 velocity;
        protected Vector2 targetVelocity;
        protected ContactFilter2D contactFilter;
        protected RaycastHit2D[] collisionResults = new RaycastHit2D[16];
        protected List<RaycastHit2D> collisions = new List<RaycastHit2D>(16);
        protected bool grounded;
        protected Vector2 groundNormal;

        // Protected methods
        // -----------------------------------
        
        protected void Move(Vector2 movement, bool yMovement)
        {
            float distance = movement.magnitude;

            if (distance < minimumMovementDistance)
            {
                return;
            }

            int collisionsCount = rigidBody.Cast(movement, contactFilter, collisionResults, distance + shellRadius);

            collisions.Clear();
            for (int i = 0; i < collisionsCount; i++)
            {
                collisions.Add(collisionResults[i]);
            }
            
            // Check normals.
            // If the normal is more than minimumNormalY, we hit the ground. 
            for (int i = 0; i < collisions.Count; i++)
            {
                Vector2 normal = collisions[i].normal;
                if (normal.y > minimumNormalY)
                {
                    grounded = true;
                    if (yMovement)
                    {
                        groundNormal = normal;
                        normal.x = 0;
                    }
                }

                float projection = Vector2.Dot(velocity, normal);
                if (projection < 0)
                {
                    velocity = velocity - (projection * normal);
                }

                float modifiedDistance = collisions[i].distance - shellRadius;
                if (modifiedDistance < distance)
                {
                    distance = modifiedDistance;
                }
                
                OnCollision(normal);
            }
            
            rigidBody.position += movement.normalized * distance;
        }

        protected virtual void ComputeVelocity()
        {
            
        }
        
        protected virtual void OnCollision(Vector2 normal)
        {
            
        }
        
        protected virtual void Start()
        {
            contactFilter.useTriggers = false;
            contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
            // TODO: Setup Physics2D's collision matrix.
        }
        
        // Private methods
        // -----------------------------------

        private void OnEnable()
        {
            rigidBody = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            targetVelocity = Vector2.zero;
            ComputeVelocity();
        }

        private void FixedUpdate()
        {
            velocity += Physics2D.gravity * gravityModifier * Time.deltaTime;
            velocity.x = targetVelocity.x;
            
            grounded = false;

            Vector2 deltaPosition = velocity * Time.deltaTime;

            Vector2 moveAlongGround = new Vector2(groundNormal.y, -groundNormal.x);
            Vector2 movement = moveAlongGround * deltaPosition.x;
            Move(movement, false);
            
            movement = Vector2.up * deltaPosition.y;
            Move(movement, true);
        }
    }
}