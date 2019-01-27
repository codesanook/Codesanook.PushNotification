using CodeSanook.Common.Modules;
using CodeSanook.Common.Web;
using CodeSanook.PushNotification.Controllers;
using Orchard.Localization;
using Orchard.UI.Navigation;

namespace CodeSanook.Swagger
{
    public class AdminMenu : INavigationProvider
    {
        public Localizer T { get; set; }
        public string MenuName => "admin"; //attach to admin panel menu

        public void GetNavigation(NavigationBuilder builder)
        {
            builder
                .Add(item => item
                    .Caption(T("Push Notification"))
                    .Position("12")
                    .Action(
                        nameof(PushNotificationController.Index),
                        MvcHelper.GetControllerName<PushNotificationController>(),
                        new { area = ModuleHelper.GetModuleName<PushNotificationController>() }
                    )
                );
        }
    }
}