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
        [SerializeField] private GameObject gamePausedPanel;
        [SerializeField] private GameObject gameWinPanel;
        [SerializeField] private Image[] playerHearts;
        [SerializeField] private Text crystalsCountText;
        [SerializeField] private Text requiredCrystalsCountText;
        [SerializeField] private Text turret1Text;
        [SerializeField] private Text turret2Text;
        [SerializeField] private Text turret3Text;
        [SerializeField] private Text towerText;

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
                //Debug.Log("UIController::Health update");
                PlayerHealthNotification healthNotification = notification as PlayerHealthNotification;
                if (healthNotification != null)
                {
                    UpdatePlayerHealth(healthNotification.heartsCount);
                }
            }

            if (notification.type == Notification.Type.PLAYER_CRYSTALS_UPDATED)
            {
                //Debug.Log("UIController::Crystals update");
                PlayerCrystalsNotification crystalsNotification = notification as PlayerCrystalsNotification;
                if (crystalsNotification != null)
                {
                    UpdatePlayerCrystals(crystalsNotification.crystalsQuantity);
                }
            }

            if (notification.type == Notification.Type.REQUIRED_CRYSTALS_UPDATED)
            {
                //Debug.Log("UIController::RequiredCrystals update");
                RequiredCrystalsNotification crystalsNotification = notification as RequiredCrystalsNotification;
                if (crystalsNotification != null)
                {
                    UpdateRequiredCrystals(crystalsNotification.requiredQuantity);
                }
            }

            if (notification.type == Notification.Type.DEFENSE_HEALTH_UPDATED)
            {
                //Debug.Log("UIController::DefenseHealth update");
                DefenseHealthNotification defenseHealthNotification = notification as DefenseHealthNotification;
                if (defenseHealthNotification != null)
                {
                    UpdateDefenseHealth(
                        defenseHealthNotification.defenseType,
                        defenseHealthNotification.rank,
                        defenseHealthNotification.GetHealthPercentage()
                    );
                }
            }

            if (notification.type == Notification.Type.GAME_PAUSED_ON)
            {
                gamePausedPanel.SetActive(true);
            }
            
            if (notification.type == Notification.Type.GAME_PAUSED_OFF)
            {
                gamePausedPanel.SetActive(false);
            }
            
            if (notification.type == Notification.Type.GAME_WIN)
            {
                gameWinPanel.SetActive(true);
            }
        }

        public void OnRetryLevelButton()
        {
            GameController.instance.RetryLevel();
        }
        
        public void OnQuitButton()
        {
            Application.Quit();
        }
        
        public void OnResumeGame()
        {
            gamePausedPanel.SetActive(false);
            GameController.instance.isPaused = false;
        }

        // Private methods
        // -----------------------------------------

        private void Start()
        {
            GameController.instance.Broadcaster.RegisterNotifiable(this, Notification.Type.GAME_OVER);
            GameController.instance.Broadcaster.RegisterNotifiable(this, Notification.Type.PLAYER_HEALTH_UPDATED);
            GameController.instance.Broadcaster.RegisterNotifiable(this, Notification.Type.PLAYER_CRYSTALS_UPDATED);
            GameController.instance.Broadcaster.RegisterNotifiable(this, Notification.Type.REQUIRED_CRYSTALS_UPDATED);
            GameController.instance.Broadcaster.RegisterNotifiable(this, Notification.Type.DEFENSE_HEALTH_UPDATED);
            GameController.instance.Broadcaster.RegisterNotifiable(this, Notification.Type.GAME_PAUSED_ON);
            GameController.instance.Broadcaster.RegisterNotifiable(this, Notification.Type.GAME_PAUSED_OFF);
            GameController.instance.Broadcaster.RegisterNotifiable(this, Notification.Type.GAME_WIN);
        }

        private void UpdatePlayerHealth(int heartsCount)
        {
            for (int i = 0; i < playerHearts.Length; i++)
            {
                //Debug.Log("UIController::UpdatePlayerHealth - Health update image " + i);
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

        private void UpdateRequiredCrystals(int quantity)
        {
            requiredCrystalsCountText.text = quantity.ToString();
        }

        private void UpdateDefenseHealth(string defenseType, int rank, int percentage)
        {
            if (defenseType.Equals("Tower"))
            {
                towerText.text = percentage + "%";
                return;
            }

            if (defenseType.Equals("Turret"))
            {
                switch (rank)
                {
                    case 1:
                        turret1Text.text = percentage + "%";
                        break;
                    case 2:
                        turret2Text.text = percentage + "%";
                        break;
                    case 3:
                        turret3Text.text = percentage + "%";
                        break;
                }
            }
        }
    }
}