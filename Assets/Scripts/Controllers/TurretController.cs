using System.Collections.Generic;
using HitEffects;
using Interfaces;
using Notifications;
using UnityEngine;

namespace Controllers
{
    public class TurretController : MonoBehaviour, IDamageable, IHealable
    {
        // Properties
        // ------------------------------------------------------

        [SerializeField] private bool isActive;
        [SerializeField] private List<GameObject> targets = new List<GameObject>();
        [SerializeField] private GameObject bulletStart;
        [SerializeField] private GameObject bulletModel;
        [SerializeField] private float fireInterval = 1.0f;
        [SerializeField] private float direction = 1.0f;
        [SerializeField] private float health = 1.0f;
    
        [Header("Bullet Effects")]
    
        [SerializeField] private float bulletDamage = 60.0f;
        [SerializeField] private float bulletFreeze = 0.0f;

        private float lastFire;
        private Animator animator;
    
        // Public methods
        // ------------------------------------------------------
        
        public void ApplyDamage(float damage)
        {
            if (damage < 0)
            {
                return;
            }

            health -= damage;

            if (health <= 0)
            {
                Notification notification = new Notification(Notification.Type.DEFENSIVE_STRUCTURE_DESTROYED);
                GameController.instance.Broadcaster.Notify(notification);
                
                EnableCollider(false);
                
                animator.Play("TurretIdle");
                animator.enabled = false;
            }
            
        }

        public bool Heal(float healthAmount)
        {
            if (healthAmount < 0)
            {
                return false;
            }

            health += healthAmount;

            return true;
        }
        
        // Private methods
        // ------------------------------------------------------

        private void Start()
        {
            animator = GetComponent<Animator>();
        }
        
        private void Update()
        {
            if (GameController.instance.isPaused)
            {
                return;
            }
        
            if (!isActive || health <= 0)
            {
                return;
            }

            if (Time.time - lastFire > fireInterval)
            {
                lastFire = Time.time;
                Fire();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            GameObject otherGameObject = other.gameObject;

            if (!otherGameObject.CompareTag("Enemy"))
            {
                return;
            }

            if (targets.Contains(otherGameObject))
            {
                return;
            }
        
            targets.Add(otherGameObject);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            GameObject otherGameObject = other.gameObject;

            if (!otherGameObject.CompareTag("Enemy"))
            {
                return;
            }

            if (!targets.Contains(otherGameObject))
            {
                return;
            }

            targets.Remove(otherGameObject);
        }

        private void Fire()
        {
            if (bulletModel == null || bulletStart == null)
            {
                return;
            }

            if (targets.Count == 0)
            {
                return;
            }

            GameObject target = targets[0];
        
            GameObject bulletObject = Instantiate(bulletModel, transform);
            bulletObject.transform.position = bulletStart.transform.position;

            BulletController bullet = bulletObject.GetComponent<BulletController>();
            bullet.direction = new Vector2(direction, 0);
            bullet.hitIgnoreTags.Add("Player");
            bullet.hitIgnoreTags.Add("DefensiveStructure");
            AddBulletEffects(bullet);
            
            animator.Play("TurretAttack");
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

        private void EnableCollider(bool enabled)
        {
            GetComponent<CapsuleCollider2D>().enabled = enabled;
        }
    }
}