using System.Collections.Generic;
using HitEffects;
using Interfaces;
using Notifications;
using UnityEngine;

namespace Controllers
{
    public class PlayerController : CharacterController, IHealable
    {
        // Properties
        // -----------------------------------
        
        [SerializeField] private GameObject bulletStart;
        [SerializeField] private GameObject bulletModel;
        [SerializeField] private float fireInterval = 1.0f;
        [SerializeField] private int hearts = 1;
        [SerializeField] private int maxHearts = 3;
        [SerializeField] private int crystals = 0;
        
        [Header("Bullet Effects")]
    
        [SerializeField] private float bulletDamage = 60.0f;
        [SerializeField] private float bulletFreeze = 0.0f;
        
        private float lastFire;
        
        // Public methods
        // -----------------------------------

        public void OnDeathAnimationEnded()
        {
            Notification notification = new Notification(Notification.Type.PLAYER_DIED);
            GameController.instance.Broadcaster.Notify(notification);
        }

        public override void ApplyDamage(float damage)
        {
            if (damage < 0)
            {
                return;
            }

            hearts--;

            NotifyHealth();
            
            if (hearts <= 0)
            {
                Die();
            }
        }
        
        public bool Heal(float healthAmount)
        {
            if (healthAmount < 0)
            {
                return false;
            }

            hearts++;
            
            NotifyHealth();

            return true;
        }
        
        public bool Heal(int healthAmount)
        {
            if (healthAmount < 0)
            {
                return false;
            }

            if (hearts + healthAmount > maxHearts)
            {
                return false;
            }
            
            hearts += healthAmount;
            
            NotifyHealth();

            return true;
        }

        public bool ReceiveCrystals(int quantity)
        {
            crystals += quantity;
            
            NotifyCrystalsQuantity();

            return true;
        }

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
            
            NotifyHealth();
            NotifyCrystalsQuantity();
        }

        protected override void AfterMovement()
        {
            base.AfterMovement();

            if (Input.GetKey(KeyCode.E))
            {
                Fire();
            }
        }

        protected override bool ShouldAllowCollision(RaycastHit2D hit, Vector2 normal, Vector2 movement)
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

        protected override void Die()
        {
            base.Die();

            GameController.instance.isPaused = true;
            
            animator.Play("Death");
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

        private void NotifyHealth()
        {
            Debug.Log("PlayerController::NotifyHealth");
            Notification notification = new PlayerHealthNotification(hearts);
            GameController.instance.Broadcaster.Notify(notification);
        }
        
        private void NotifyCrystalsQuantity()
        {
            Debug.Log("PlayerController::NotifyCrystals");
            Notification notification = new PlayerCrystalsNotification(crystals);
            GameController.instance.Broadcaster.Notify(notification);
        }
    }
}