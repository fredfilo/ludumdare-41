namespace Notifications
{
    public class Notification
    {
        public enum Type
        {
            GAME_OVER,
            TO_PROTECT_DESTROYED,
            DEFENSIVE_STRUCTURE_DESTROYED,
            PLAYER_DIED,
            PLAYER_HEALTH_UPDATED,
            PLAYER_CRYSTALS_UPDATED,
            REQUIRED_CRYSTALS_UPDATED
        }
    
        // Properties
        // --------------------------------------------

        public Type type;
        
        // Constructors
        // --------------------------------------------

        public Notification(Type type)
        {
            this.type = type;
        }
    }
}