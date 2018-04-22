using System.Collections.Generic;
using HitEffects;
using Interfaces;
using Physics;
using UnityEngine;

namespace Controllers
{
    public class BulletController : MonoBehaviour
    {
        // Properties
        // --------------------------------------------

        public Vector2 direction;
        public float timeBeforeExplosion = 2.0f;
        public float speed = 1.0f;
        public List<HitEffect> hitEffects = new List<HitEffect>();
        public List<string> hitIgnoreTags = new List<string>();
        public bool isFriendly = false;

        [SerializeField] private GameObject explosionModel;

        // Private methods
        // --------------------------------------------

        private void Awake()
        {
            hitIgnoreTags.Add("Projectile");
        }

        private void Update()
        {
            if (GameController.instance.isPaused)
            {
                return;
            }
            
            timeBeforeExplosion -= Time.deltaTime;
            if (timeBeforeExplosion <= 0)
            {
                Destroy(gameObject);
            }
            
            Vector3 position = transform.position;
            position.x += direction.x * speed * Time.deltaTime;
            position.y += direction.y * speed * Time.deltaTime;
            transform.position = position;
        }

        private void OnCollisionEnter2D(Collision2D collider)
        {
            GameObject otherGameObject = collider.gameObject;

            if (hitIgnoreTags.Contains(otherGameObject.tag))
            {
                return;
            }
            
            foreach (HitEffect hitEffect in hitEffects)
            {
                hitEffect.Apply(otherGameObject);
            }
            
            Explode();
        }

        private void Explode()
        {
            if (explosionModel != null)
            {
                GameObject explosionObject = Instantiate(explosionModel);
                explosionObject.transform.position = new Vector3(
                    transform.position.x + Random.Range(-0.2f, 0.2f),
                    transform.position.y + Random.Range(-0.2f, 0.2f),
                    0
                );
            }
            
            Destroy(gameObject);
        }
    }
}