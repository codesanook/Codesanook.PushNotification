using CodeSanook.PushNotification.Models;
using Newtonsoft.Json;
using Orchard.Users.Models;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web.Http;
using Orchard.ContentManagement;
using Orchard;
using System.Dynamic;

namespace CodeSanook.PushNotification.Controllers
{
    public class PushNotificationsController : ApiController
    {
        private readonly IContentManager contentManager;
        private readonly IOrchardServices orchardService;

        public PushNotificationsController(
            IOrchardServices orchardService,
            IContentManager contentManager
        )
        {
            this.orchardService = orchardService;
            this.contentManager = contentManager;
        }

        public void Post(PushNotificationMessage request)
        {
            var user = contentManager
                .Query<UserPart, UserPartRecord>()
                .Where(u => u.Email == request.Email)
                .List()
                .SingleOrDefault();

            if (user == null)
            {
                throw new InvalidOperationException($"No user with email {request.Email}");
            }

            var pushNotificationPart = user.As<PushNotificationPart>();
            SendGCMNotification(pushNotificationPart, request);
        }

        private string SendGCMNotification(PushNotificationPart pushNotificationPart, PushNotificationMessage message)
        {
            dynamic payload = new ExpandoObject();
            payload.registration_ids = new[] { pushNotificationPart.RegistrationId };
            payload.priority = "high";
            const string soundName = "default";

            switch (pushNotificationPart.Platform.ToUpperInvariant())
            {
                case "IOS":
                    payload.notification = new
                    {
                        title = message.Title,
                        body = message.Body,
                        sound = soundName,
                    };
                    break;
                case "ANDROID"://work for start up and get push in background
                    payload.data = new
                    {
                        title = message.Title,
                        body = message.Body,
                        soundname  = soundName,
                    };
                    break;
                default:
                    throw new InvalidCastException("no valid registration platform");
            }

            var payloadJson = JsonConvert.SerializeObject(payload);
            var payloadData = Encoding.UTF8.GetBytes(payloadJson);

            ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(ValidateServerCertificate);
            var request = (HttpWebRequest)WebRequest.Create("https://fcm.googleapis.com/fcm/send");
            request.Method = "POST";
            request.KeepAlive = false;
            request.ContentType = "application/json";

            var setting = this.orchardService.WorkContext.CurrentSite.As<PushNotificationSettingPart>();
            request.Headers.Add($"Authorization: key={setting.ApiKey}");
            // We don't need header of Sender: id=xxx but some examples on the internet add it.
            request.ContentLength = payloadData.Length;

            WebResponse response;
            using (var dataStream = request.GetRequestStream())
            {
                dataStream.Write(payloadData, 0, payloadData.Length);
                response = request.GetResponse();
            }
            var responseCode = ((HttpWebResponse)response).StatusCode;
            if (responseCode.Equals(HttpStatusCode.Unauthorized) || responseCode.Equals(HttpStatusCode.Forbidden))
            {
                throw new InvalidOperationException("Unauthorized - need new token");
            }
            else if (!responseCode.Equals(HttpStatusCode.OK))
            {
                throw new InvalidOperationException("Response from web service isn't OK");
            }

            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                var responseLine = reader.ReadToEnd();
                return responseLine;
            }
        }

        public static bool ValidateServerCertificate(
          object sender,
          X509Certificate certificate,
          X509Chain chain,
          SslPolicyErrors sslPolicyErrors
        ) => true;
    }
}