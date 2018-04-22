using UnityEngine;

namespace Controllers
{
    public class ExplosionController : MonoBehaviour
    {
        // Public methods
        // --------------------------------

        public void OnAnimationComplete()
        {
            Destroy(gameObject);
        }

        // Private methods
        // --------------------------------

        private void Start()
        {
            SpriteRenderer renderer = GetComponent<SpriteRenderer>();
            renderer.flipX = Random.Range(-1f, 1f) > 0;
            renderer.flipY = Random.Range(-1f, 1f) > 0;
        }
    }
}