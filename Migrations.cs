using Orchard.Data.Migration;
using Codesanook.PushNotification.Models;
using Codesanook.Common.Data;
using System;
using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Users.Models;
using Codesanook.Common.Models;

namespace Codesanook.PushNotification
{
    public class Migrations : DataMigrationImpl
    {
        public int Create()
        {
            SchemaBuilder.CreateTable<PushNotificationPartRecord>(
                tableCommand => tableCommand
                    .ContentPartRecord()
                    .Column<PushNotificationPartRecord, string>(table => table.RegistrationId)
                    .Column<PushNotificationPartRecord, string>(table => table.Platform)
                    .Column<PushNotificationPartRecord, DateTime?>(table => table.UpdatedUtc)
            );

            //attach to user content type
            ContentDefinitionManager.AlterPartDefinition(
                nameof(PushNotificationPart), build => build.Attachable(true)
            );

            ContentDefinitionManager.AlterTypeDefinition<UserPart>(
                builder => builder.WithPart(nameof(PushNotificationPart))
            );

            return 1;
        }
        
        public int UpdateFrom1()
        {
            SchemaBuilder.CreateTable<PushNotificationSettingPartRecord>(
                tableCommand => tableCommand
                    .ContentPartRecord()
                    .Column<PushNotificationSettingPartRecord, string>(table => table.ApiKey)
            );

            return 2;
        }
    }

}
