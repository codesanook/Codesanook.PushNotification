using Orchard.ContentManagement.Records;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeSanook.PushNotification.Models
{
    public class PushNotificationSettingPartRecord:ContentPartRecord
    {
        public string ApiKey { get; set; }
    }
}