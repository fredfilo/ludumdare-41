namespace Notifications
{
    public class PlayerHealthNotification : Notification
    {
        // Properties
        // -----------------------------------
        
        public int heartsCount = 0;
        
        // Constructors
        // -----------------------------------
        
        public PlayerHealthNotification(int heartsCount) : base(Type.PLAYER_HEALTH_UPDATED)
        {
            this.heartsCount = heartsCount;
        }
    }
}