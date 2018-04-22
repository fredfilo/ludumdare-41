using Interfaces;
using Notifications;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers
{
    public class GameUIController : MonoBehaviour, INotifiable
    {
        // Properties
        // -----------------------------------------

        [SerializeField] private GameObject gameOverPanel;
        [SerializeField] private Image[] playerHearts;
        [SerializeField] private Text crystalsCountText;

        // Public methods
        // -----------------------------------------
        
        public void OnNotification(Notification notification)
        {
            if (notification.type == Notification.Type.GAME_OVER)
            {
                gameOverPanel.SetActive(true);
            }
            
            if (notification.type == Notification.Type.PLAYER_HEALTH_UPDATED)
            {
                Debug.Log("UIController::Health update");
                PlayerHealthNotification healthNotification = notification as PlayerHealthNotification;
                if (healthNotification != null)
                {
                    UpdatePlayerHealth(healthNotification.heartsCount);
                }
            }
            
            if (notification.type == Notification.Type.PLAYER_CRYSTALS_UPDATED)
            {
                Debug.Log("UIController::Health update");
                PlayerCrystalsNotification crystalsNotification = notification as PlayerCrystalsNotification;
                if (crystalsNotification != null)
                {
                    UpdatePlayerCrystals(crystalsNotification.crystalsQuantity);
                }
            }
        }

        public void OnRetryLevelButton()
        {
            GameController.instance.RetryLevel();
        }
        
        // Private methods
        // -----------------------------------------
        
        private void Awake()
        {
            GameController.instance.Broadcaster.RegisterNotifiable(this, Notification.Type.GAME_OVER);
            GameController.instance.Broadcaster.RegisterNotifiable(this, Notification.Type.PLAYER_HEALTH_UPDATED);
            GameController.instance.Broadcaster.RegisterNotifiable(this, Notification.Type.PLAYER_CRYSTALS_UPDATED);
        }

        private void UpdatePlayerHealth(int heartsCount)
        {
            for (int i = 0; i < playerHearts.Length; i++)
            {
                Debug.Log("UIController::UpdatePlayerHealth - Health update image " + i);
                Image heartImage = playerHearts[i];
                Color imageColor = heartImage.color;
                imageColor.a = i < heartsCount ? 1.0f : 0.2f;
                heartImage.color = imageColor;
            }
        }

        private void UpdatePlayerCrystals(int quantity)
        {
            crystalsCountText.text = quantity.ToString();
        }
    }
}