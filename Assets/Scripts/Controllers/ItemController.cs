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
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Player"))
            {
                return;
            }
            
            animator.Play("Pickup");
        }
    }
}