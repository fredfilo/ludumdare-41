using System.Collections;
using UnityEngine;

namespace Controllers
{
    public class ItemController : MonoBehaviour
    {
        // Properties
        // -----------------------------------

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
            if (!other.gameObject.CompareTag("Player"))
            {
                return;
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