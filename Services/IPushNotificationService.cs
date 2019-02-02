using CodeSanook.PushNotification.Controllers;
using CodeSanook.PushNotification.Models;
using Orchard;

namespace CodeSanook.PushNotification.Services
{
    public interface IPushNotificationService : IDependency
    {
        void SendPush(PushNotificationPart pushNotificationParts, PushNotificationMessage message);
    }
}