using Codesanook.PushNotification.Drivers;
using Codesanook.PushNotification.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using Orchard.Localization;

namespace Codesanook.PushNotification.Handlers
{
    public class PushNotificationPartHandler : ContentHandler
    {
        public PushNotificationPartHandler(IRepository<PushNotificationPartRecord> repository)
        {
            Filters.Add(StorageFilter.For(repository));
        }
    }
}