using Codesanook.PushNotification.Controllers;
using Codesanook.PushNotification.Models;
using Orchard;

namespace Codesanook.PushNotification.Services
{
    public interface IPushNotificationService : IDependency
    {
        void SendPush(PushNotificationPart pushNotificationParts, PushNotificationMessage message);
    }
}