namespace Notifications
{
    public class DefenseDestroyedNotification : Notification
    {
        // Properties
        // ------------------------------
        
        public string defenseType;
        public int rank;
        
        // Constructors
        // ------------------------------
        
        public DefenseDestroyedNotification(string defenseType, int rank = 1) : base(Type.DEFENSIVE_STRUCTURE_DESTROYED)
        {
            this.defenseType = defenseType;
            this.rank = rank;
        }
    }
}