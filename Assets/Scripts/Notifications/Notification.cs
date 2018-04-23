namespace Notifications
{
    public class Notification
    {
        public enum Type
        {
            GAME_OVER,
            GAME_PAUSED_ON,
            GAME_PAUSED_OFF,
            GAME_WIN,
            TO_PROTECT_DESTROYED,
            DEFENSIVE_STRUCTURE_DESTROYED,
            PLAYER_DIED,
            PLAYER_HEALTH_UPDATED,
            PLAYER_CRYSTALS_UPDATED,
            REQUIRED_CRYSTALS_UPDATED,
            DEFENSE_HEALTH_UPDATED
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