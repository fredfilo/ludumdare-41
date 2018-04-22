using System.Collections.Generic;
using Interfaces;
using UnityEngine;

namespace Notifications
{
    public class Broadcaster
    {
        // Properties
        // -----------------------------------
        
        private Dictionary<Notification.Type, List<INotifiable>> notifiablesDictionary = new Dictionary<Notification.Type, List<INotifiable>>();
        
        // Public methods
        // -----------------------------------

        public void RegisterNotifiable(INotifiable notifiable, Notification.Type type)
        {
            List<INotifiable> notifiables = GetNotifiablesByType(type);
            if (notifiables == null)
            {
                notifiables = new List<INotifiable>();
                notifiablesDictionary[type] = notifiables;
            }

            if (!notifiables.Contains(notifiable))
            {
                //Debug.Log("Broadcaster::RegisterNotifiable (type=" + type + ")");
                notifiables.Add(notifiable);
            }
        }
        
        public void RemoveNotifiable(INotifiable notifiable, Notification.Type type)
        {
            List<INotifiable> notifiables = GetNotifiablesByType(type);
            if (notifiables == null)
            {
                return;
            }

            notifiables.Remove(notifiable);
        }

        public void Notify(Notification notification)
        {
            List<INotifiable> notifiables = GetNotifiablesByType(notification.type);
            if (notifiables == null)
            {
                //Debug.Log("Broadcaster::Notify " + notification.type + " : Notifiable list is null.");
                return;
            }

            //Debug.Log("Broadcaster::Notify " + notification.type + " : " + notifiables.Count);
            
            foreach (INotifiable notifiable in notifiables)
            {
                notifiable.OnNotification(notification);
            }
        }

        // Private methods
        // -----------------------------------
        
        private List<INotifiable> GetNotifiablesByType(Notification.Type type)
        {
            List<INotifiable> notifiables = null;

            try
            {
                notifiables = notifiablesDictionary[type];
            }
            catch (KeyNotFoundException exception) {}

            return notifiables;
        }
    }
}