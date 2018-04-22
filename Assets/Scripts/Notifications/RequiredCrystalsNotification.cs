namespace Notifications
{
    public class RequiredCrystalsNotification : Notification
    {
        // Properties
        // --------------------------------

        public int requiredQuantity;
        
        // Constructors
        // --------------------------------
        
        public RequiredCrystalsNotification(int quantity) : base(Type.REQUIRED_CRYSTALS_UPDATED)
        {
            requiredQuantity = quantity;
        }
    }
}