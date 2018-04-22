using UnityEngine;

namespace Controllers
{
    public class CameraController : MonoBehaviour
    {
        // Properties
        // ----------------------------------

        public GameObject target;
        
        // Private methods
        // ----------------------------------
        
        private void LateUpdate()
        {
            if (target == null)
            {
                return;
            }

            Vector3 targetPosition = target.transform.position;
            targetPosition.y += 3;
            transform.position = targetPosition;
        }
    }
}