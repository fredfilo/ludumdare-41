namespace Notifications
{
    public class PlayerCrystalsNotification : Notification
    {
        // Properties
        // ---------------------------------

        public int crystalsQuantity;
        
        // Constructors
        // ---------------------------------
        
        public PlayerCrystalsNotification(int crystals) : base(Type.PLAYER_CRYSTALS_UPDATED)
        {
            crystalsQuantity = crystals;
        }
    }
}