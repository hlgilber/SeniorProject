using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RestSharp;

namespace GraceChurchKelseyvilleAwana.Email
{
    public class EmailHelper
    {
        public static void SendEmail(string subject, string body, string recipient)
        {
            RestClient client = new RestClient();
            client.BaseUrl = "https://api.mailgun.net/v2";
            client.Authenticator = new HttpBasicAuthenticator("api", "key-7lenghibhs19ph3bfqfrw44tccoszyt9");
            RestRequest request = new RestRequest();
            request.AddParameter("domain", "app78b6edd86d874ea4a95792eba7d8adab.mailgun.org", ParameterType.UrlSegment);
            request.Resource = "{domain}/messages";
            request.AddParameter("from", "Grace Church Kelseyville Awana <do-not-reply@gcka.apphb.com>");
            request.AddParameter("to", recipient);
            request.AddParameter("subject", subject);
            request.AddParameter("text", "This is a test email sent from the mailgun api." + (string.IsNullOrEmpty(body) ? "" : "\n\n" + body));
            request.Method = Method.POST;
            var result = client.Execute(request);
        }

    }
}