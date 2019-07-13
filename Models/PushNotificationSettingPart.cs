using Orchard.ContentManagement;
using System.ComponentModel.DataAnnotations;

namespace Codesanook.PushNotification.Models
{
    public class PushNotificationSettingPart : ContentPart<PushNotificationSettingPartRecord>
    {
        [Required]
        public string ApiKey
        {
            get { return this.Record.ApiKey; }
            set { this.Record.ApiKey = value; }
        }
    }
}