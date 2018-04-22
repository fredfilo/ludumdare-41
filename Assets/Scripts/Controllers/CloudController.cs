using UnityEngine;

namespace Controllers
{
    public class CloudController : MonoBehaviour
    {
        // Properties
        // ------------------------------------

        [SerializeField] private float speed = 0.1f;
        [SerializeField] private float runDistance = 30.0f;

        private float disappearX;
        private float spawnX;
        
        // Private methods
        // ------------------------------------

        private void Start()
        {
            disappearX = -runDistance / 2.0f;
            spawnX = runDistance / 2.0f;
        }
        
        private void Update()
        {
            Vector3 position = transform.position;
            position.x -= speed * Time.deltaTime;

            if (position.x <= disappearX)
            {
                position.x = spawnX;
            }
            
            transform.position = position;
        }
    }
}