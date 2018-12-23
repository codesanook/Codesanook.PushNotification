using CodeSanook.PushNotification.Models;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;

namespace CodeSanook.PushNotification.Handlers
{
    public class PushNotificationPartHandler:ContentHandler
    {
        public PushNotificationPartHandler(IRepository<PushNotificationPartRecord> repository)
        {
            Filters.Add(StorageFilter.For(repository));
        }
    }
}