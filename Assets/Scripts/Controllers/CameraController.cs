using UnityEngine;

namespace Controllers
{
    public class CameraController : MonoBehaviour
    {
        // Properties
        // ----------------------------------

        public GameObject target;
        public bool followsTarget = true;
        public float followSpeed = 0.3f;
        
        // Private methods
        // ----------------------------------
        
        private void LateUpdate()
        {
            if (!followsTarget || target == null)
            {
                return;
            }

            Vector3 targetPosition = target.transform.position;
            
            Vector3 newPosition = transform.position;
            newPosition.x = targetPosition.x;
            newPosition.y = targetPosition.y + 3;
            transform.position = Vector3.Lerp(transform.position, newPosition, followSpeed);
        }
    }
}