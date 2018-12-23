using Newtonsoft.Json;
using Orchard.ContentManagement.Records;
using System;

namespace CodeSanook.PushNotification.Models
{
    public class PushNotificationPartRecord:ContentPartRecord
    {
        public virtual string RegistrationId { get; set; }
        public virtual string Platform { get; set; }
        public virtual DateTime? UpdatedUtc { get; set; }
    }
}