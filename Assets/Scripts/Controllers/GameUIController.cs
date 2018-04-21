using Interfaces;
using Notifications;
using UnityEngine;

namespace Controllers
{
    public class GameUIController : MonoBehaviour, INotifiable
    {
        // Properties
        // -----------------------------------------

        [SerializeField] private GameObject gameOverPanel;

        // Public methods
        // -----------------------------------------
        
        public void OnNotification(Notification notification)
        {
            if (notification.type == Notification.Type.GAME_OVER)
            {
                gameOverPanel.SetActive(true);
            }
        }

        public void OnRetryLevelButton()
        {
            GameController.instance.RetryLevel();
        }
        
        // Private methods
        // -----------------------------------------
        
        private void Start()
        {
            GameController.instance.Broadcaster.RegisterNotifiable(this, Notification.Type.GAME_OVER);
        }
    }
}