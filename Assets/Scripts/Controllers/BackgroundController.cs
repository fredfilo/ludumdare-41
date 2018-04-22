using System.Collections.Generic;
using UnityEngine;

namespace Controllers
{
    public class BackgroundController : MonoBehaviour
    {
        // Properties
        // -------------------------------------
        
        private Vector2 distanceFromCamera;
        
        // Private methods
        // -------------------------------------

        private void Start()
        {
            Vector3 cameraPosition = Camera.main.transform.position;
            distanceFromCamera = new Vector2(
                transform.position.x - cameraPosition.x,
                transform.position.y - cameraPosition.y
            );
        }
        
        private void LateUpdate()
        {
            Vector3 cameraPosition = Camera.main.transform.position;
            
            Vector3 position = transform.position;
            position.x = cameraPosition.x + distanceFromCamera.x;
            position.y = cameraPosition.y + distanceFromCamera.y;
            transform.position = position;
        }
    }
}