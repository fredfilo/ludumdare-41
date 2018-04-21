namespace Notifications
{
    public class Notification
    {
        public enum Type
        {
            GAME_OVER,
            TO_PROTECT_DESTROYED
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