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
            
            transform.position = target.transform.position;
        }
    }
}