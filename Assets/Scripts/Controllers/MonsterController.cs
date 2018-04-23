using System;
using System.Collections.Generic;
using HitEffects;
using Interfaces;
using UnityEngine;

namespace Controllers
{
    public class MonsterController : MonoBehaviour, IDamageable
    {
        [SerializeField] public float direction = 1.0f;
        [SerializeField] public float health = 1.0f;

        [SerializeField] private GameObject bulletStart;
        [SerializeField] private GameObject bulletModel;
        [SerializeField] public float fireSpeed = 1.0f;
        [SerializeField] public float fireDamage = 1.0f;
        [SerializeField] public float fireFreeze = 0.0f;
        
        [Header("Patrol")]
        
        [SerializeField] public float patrolSpeed;
        [SerializeField] public GameObject patrolLeft;
        [SerializeField] public GameObject patrolRight;

        private float lastFire;
        private SpriteRenderer renderer;
        private GameObject fireTarget;
        public float patrolXMin;
        public float patrolXMax;
        public float patrolTarget;

        private void Start()
        {
            renderer = GetComponent<SpriteRenderer>();

            patrolXMin = patrolLeft.transform.position.x;
            patrolXMax = patrolRight.transform.position.x;
        }

        private void Update()
        {
            if (fireTarget != null)
            {
                Fire(fireTarget);
                return;
            }
            
            // Patrol
            if (patrolTarget != patrolXMin && patrolTarget != patrolXMax)
            {
                patrolTarget = patrolXMin;
                direction = -1.0f;
            }

            if (Math.Abs(patrolTarget - patrolXMin) < 0.00001f && transform.position.x < patrolTarget)
            {
                direction = 1.0f;
                patrolTarget = patrolXMax;
            }
            else if (Math.Abs(patrolTarget - patrolXMax) < 0.00001f && transform.position.x > patrolTarget)
            {
                direction = -1.0f;
                patrolTarget = patrolXMin;
            }

            renderer.flipX = direction < 0;
            
            Vector3 newPosition = transform.position;
            newPosition.x += direction * patrolSpeed * Time.deltaTime;
            transform.position = newPosition;
        }
        
        private void Die()
        {
            Destroy(gameObject);
        }

        public void ApplyDamage(float damage)
        {
            if (damage < 0)
            {
                return;
            }

            health -= damage;

            if (health <= 0)
            {
                Die();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Player"))
            {
                return;
            }

            fireTarget = other.gameObject;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (fireTarget != null && other.gameObject == fireTarget)
            {
                fireTarget = null;
            }
        }

        private void Fire(GameObject target)
        {
            //Debug.Log("Monster fire");
            if (Time.time - lastFire < fireSpeed)
            {
                return;
            }

            lastFire = Time.time;
                
            GameObject bulletObject = Instantiate(bulletModel);
            bulletObject.transform.position = bulletStart.transform.position;

            Vector2 direction = new Vector2(
                target.transform.position.x - bulletStart.transform.position.x,
                target.transform.position.y - bulletStart.transform.position.y
            );
            
            BulletController bullet = bulletObject.GetComponent<BulletController>();
            bullet.direction = direction.normalized;
            bullet.speed *= 1.25f;
            bullet.hitIgnoreTags.Add("Enemy");
            bullet.hitIgnoreTags.Add("Monster");
            bullet.hitIgnoreTags.Add("DefensiveStructure");
            AddBulletEffects(bullet);
        }
        
        private void AddBulletEffects(BulletController bullet)
        {
            if (bullet == null)
            {
                return;
            }
        
            if (fireDamage > 0)
            {
                bullet.hitEffects.Add(new DamageEffect(fireDamage));
            }
        
            if (fireFreeze > 0)
            {
                bullet.hitEffects.Add(new FreezeEffect(fireFreeze));
            }
        }
    }
}