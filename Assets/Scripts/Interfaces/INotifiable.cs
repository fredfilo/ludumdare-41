using Notifications;

namespace Interfaces
{
    public interface INotifiable
    {
        void OnNotification(Notification notification);
    }
}