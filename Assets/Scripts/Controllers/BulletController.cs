﻿using System.Collections.Generic;
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

        // Private methods
        // --------------------------------------------
        
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

            foreach (HitEffect hitEffect in hitEffects)
            {
                hitEffect.Apply(otherGameObject);
            }
            
            Destroy(gameObject);
        }
    }
}