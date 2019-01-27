using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.UI.Notify;
using Orchard.Localization;
using CodeSanook.PushNotification.Models;

namespace CodeSanook.PushNotification.Drivers
{
    public class PushNotificationSettingPartDriver : ContentPartDriver<PushNotificationSettingPart>
    {
        private readonly INotifier notifier;
        protected override string Prefix => "PushNotificationSetting";
        public Localizer T { get; set; }
        public const string GroupNameToRenderEditor = "Push Notification";

        public PushNotificationSettingPartDriver(INotifier notifier)
        {
            this.notifier = notifier;
            this.T = NullLocalizer.Instance;
        }

        protected override DriverResult Editor(PushNotificationSettingPart part, dynamic shapeHelper)
        {
            return ContentShape("Parts_PushNotificationSetting",
                () => shapeHelper.EditorTemplate(
                    TemplateName: "Parts/PushNotificationSetting",
                    Model: part,
                    Prefix: Prefix
                ))
                .OnGroup(GroupNameToRenderEditor);//Show in Push Notification Setting group
        }

        protected override DriverResult Editor(PushNotificationSettingPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            updater.TryUpdateModel(part, Prefix, null, null);
            //show edited content
            return Editor(part, shapeHelper);
        }
    }
}