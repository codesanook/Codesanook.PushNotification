using Newtonsoft.Json;
using Orchard.ContentManagement;
using System;

namespace CodeSanook.PushNotification.Models
{
    public class PushNotificationPart : ContentPart<PushNotificationPartRecord>
    {
        public string RegistrationId
        {
            get { return this.Record.RegistrationId; }
            set { this.Record.RegistrationId = value; }
        }

        public string Platform
        {
            get { return this.Record.Platform; }
            set { this.Record.Platform = value; }
        }

        public DateTime? UpdatedUtc
        {
            get { return this.Record.UpdatedUtc; }
            set { this.Record.UpdatedUtc = value; }
        }
    }
}