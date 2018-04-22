using System.Collections.Generic;
using Controllers;
using UnityEngine;

namespace Physics
{
    public class MovableObject : MonoBehaviour
    {
        // Properties
        // -----------------------------------

        [Header("Physics")]
        
        public float gravityModifier = 1.0f;
        public float minimumNormalY = 0.65f; // 0.65f
        
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
        protected bool isEnemy = false;

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
                RaycastHit2D collision = collisions[i];
                Vector2 normal = collision.normal;
                if (normal.y > minimumNormalY)
                {
                    grounded = true;
                    if (yMovement)
                    {
                        groundNormal = normal;
                        normal.x = 0;
                    }
                }

                if (ShouldAllowCollision(collision, normal, movement))
                {
                    continue;
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
            }

            if (isEnemy)
            {
                // TODO: Really need to fix this.
                if (movement.y > 0)
                {
                    movement.y = 0;
                }
            }

            float yBefore = rigidBody.position.y;
            
            rigidBody.position += movement.normalized * distance;

            if (isEnemy && rigidBody.position.y > yBefore)
            {
                rigidBody.position = new Vector3(
                    rigidBody.position.x,
                    yBefore
                );
            }
        }

        protected virtual void ComputeVelocity()
        {
            
        }
        
        protected virtual void AfterMovement()
        {
            
        }
        
        protected virtual bool ShouldAllowCollision(RaycastHit2D hit, Vector2 normal, Vector2 movement)
        {
            return false;
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
            if (GameController.instance.isPaused)
            {
                return;
            }
            
            targetVelocity = Vector2.zero;
            ComputeVelocity();
            AfterMovement();
        }

        private void FixedUpdate()
        {
            if (GameController.instance.isPaused)
            {
                return;
            }
            
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