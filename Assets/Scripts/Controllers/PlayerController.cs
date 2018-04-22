using System.Collections.Generic;
using HitEffects;
using UnityEngine;

namespace Controllers
{
    public class PlayerController : CharacterController
    {
        // Properties
        // -----------------------------------
        
        [SerializeField] private GameObject bulletStart;
        [SerializeField] private GameObject bulletModel;
        [SerializeField] private float fireInterval = 1.0f;
        
        [Header("Bullet Effects")]
    
        [SerializeField] private float bulletDamage = 60.0f;
        [SerializeField] private float bulletFreeze = 0.0f;
        
        private float lastFire;
        private List<string> allowedCollisionTags = new List<string>()
        {
            "Enemy",
            "ToProtect",
            "DefensiveStructure"
        };
        
        // Protected methods
        // -----------------------------------

        protected override void Start()
        {
            base.Start();

            allowedCollisionTags = new List<string>()
            {
                "Enemy",
                "Projectile",
                "ToProtect",
                "DefensiveStructure"
            };
        }

        protected override void AfterMovement()
        {
            base.AfterMovement();

            if (Input.GetKey(KeyCode.E))
            {
                Fire();
            }
        }

        protected override bool ShouldAllowCollision(RaycastHit2D hit, Vector2 normal)
        {
            GameObject otherGameObject = hit.collider.gameObject;

            if (allowedCollisionTags.Contains(otherGameObject.tag))
            {
                return true;
            }
           
            return false;
        }

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

        // Private methods
        // -----------------------------------
        
        private void Fire()
        {
            if (Time.time - lastFire < fireInterval)
            {
                return;
            }
            
            lastFire = Time.time;
            
            GameObject bulletObject = Instantiate(bulletModel);
            bulletObject.transform.position = bulletStart.transform.position;

            BulletController bullet = bulletObject.GetComponent<BulletController>();
            bullet.direction = new Vector2(renderer.flipX ? -1.0f : 1.0f, 0);
            bullet.speed *= 1.25f;
            bullet.hitIgnoreTags.Add("Player");
            bullet.hitIgnoreTags.Add("DefensiveStructure");
            AddBulletEffects(bullet);
        }
        
        private void AddBulletEffects(BulletController bullet)
        {
            if (bullet == null)
            {
                return;
            }
        
            if (bulletDamage > 0)
            {
                bullet.hitEffects.Add(new DamageEffect(bulletDamage));
            }
        
            if (bulletFreeze > 0)
            {
                bullet.hitEffects.Add(new FreezeEffect(bulletFreeze));
            }
        }
    }
}