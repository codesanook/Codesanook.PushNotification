using CodeSanook.PushNotification.Drivers;
using CodeSanook.PushNotification.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using Orchard.Localization;

namespace CodeSanook.PushNotification.Handlers
{
    public class PushNotificationSettingPartHandler : ContentHandler
    {
        public Localizer T { get; set; }

        public PushNotificationSettingPartHandler(IRepository<PushNotificationSettingPartRecord> repository)
        {
            Filters.Add(StorageFilter.For(repository));
            //attach part to content item
            Filters.Add(new ActivatingFilter<PushNotificationSettingPart>("Site"));
            T = NullLocalizer.Instance;
        }

        protected override void GetItemMetadata(GetContentItemMetadataContext context)
        {
            if (context.ContentItem.ContentType != "Site") return;

            base.GetItemMetadata(context);
            context.Metadata.EditorGroupInfo.Add(
                new GroupInfo(T(PushNotificationSettingPartDriver.GroupNameToRenderEditor))
            );
        }
    }
}