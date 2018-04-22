using UnityEngine;

namespace Notifications
{
    public class DefenseHealthNotification : Notification
    {
        // Properties
        // -------------------------------

        public float health;
        public float maxHealth;
        public string defenseType;
        public int rank;

        public DefenseHealthNotification(float health, float maxHealth, string defenseType, int rank = 1) : base(Type.DEFENSE_HEALTH_UPDATED)
        {
            this.health = health;
            this.maxHealth = maxHealth;
            this.defenseType = defenseType;
            this.rank = rank;
        }

        public int GetHealthPercentage()
        {
            if (maxHealth <= 0)
            {
                return 0;
            }

            return Mathf.RoundToInt(100.0f * (health / maxHealth));
        }
    }
}