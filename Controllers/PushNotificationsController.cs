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
            SendGCMNotification(pushNotificationPart.RegistrationId, request);
        }

        private string SendGCMNotification(string registrationId, PushNotificationMessage pushNotificationMessage)
        {
            var message = new
            {
                notification = new
                {
                    sound = "default",
                    title = pushNotificationMessage.Title,
                    body = pushNotificationMessage.Body,
                    someKey = "someValue"
                },
                registration_ids = new[] { registrationId },
                priority = "high",
                content_available = true,
                mutable_content = true
            };

            //  MESSAGE CONTENT
            byte[] byteArray = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

            //  CREATE REQUEST
            ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(ValidateServerCertificate);
            HttpWebRequest Request = (HttpWebRequest)WebRequest.Create("https://android.googleapis.com/gcm/send");
            Request.Method = "POST";
            Request.KeepAlive = false;
            Request.ContentType = "application/json";

            var setting = this.orchardService.WorkContext.CurrentSite.As<PushNotificationSettingPart>();
            Request.Headers.Add($"Authorization: key={setting.ApiKey}");
            Request.ContentLength = byteArray.Length;

            var dataStream = Request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);

            dataStream.Close();
            //  SEND MESSAGE
            try
            {
                WebResponse Response = Request.GetResponse();
                HttpStatusCode ResponseCode = ((HttpWebResponse)Response).StatusCode;
                if (ResponseCode.Equals(HttpStatusCode.Unauthorized) || ResponseCode.Equals(HttpStatusCode.Forbidden))
                {
                    Console.WriteLine("Unauthorized - need new token");
                }

                else if (!ResponseCode.Equals(HttpStatusCode.OK))
                {
                    Console.WriteLine("Response from web service isn't OK");
                }

                var reader = new StreamReader(Response.GetResponseStream());
                string responseLine = reader.ReadToEnd();
                reader.Close();
                return responseLine;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message); ;
                Console.WriteLine(e.StackTrace); ;
            }
            return "error";
        }

        public static bool ValidateServerCertificate(
          object sender,
          X509Certificate certificate,
          X509Chain chain,
          SslPolicyErrors sslPolicyErrors
        ) => true;
    }
}