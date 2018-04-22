using System.Collections;
using UnityEngine;

namespace Controllers
{
    public class ItemController : MonoBehaviour
    {
        // Properties
        // -----------------------------------

        [SerializeField] private int health = 0;
        [SerializeField] private int crystals = 0;
        
        private Animator animator;
        
        // Public methods
        // -----------------------------------

        public void OnPickupAnimationEnded()
        {
            Destroy(gameObject);
        }
        
        // Private methods
        // -----------------------------------
        
        private void Start()
        {
            animator = GetComponent<Animator>();
            animator.enabled = false;
            StartCoroutine(DelayAnimation());
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            
            if (player == null)
            {
                return;
            }
            
            if (health > 0)
            {
                player.Heal(health);
            }

            if (crystals > 0)
            {
                player.ReceiveCrystals(crystals);
            }
            
            animator.Play("Pickup");
        }

        private IEnumerator DelayAnimation()
        {
            float startAt = Time.time + Random.Range(0f, 1f);
            while (Time.time < startAt)
            {
                yield return null;
            }

            animator.enabled = true;
        }
    }
}