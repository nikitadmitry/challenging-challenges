using System;
using System.Net;

namespace Business.CodeExecution.Private
{
    public class CookieAwareWebClient : WebClient
    {
        public CookieAwareWebClient()
        {
            CookieContainer = new CookieContainer();
            ResponseCookies = new CookieCollection();
        }

        public CookieContainer CookieContainer
        {
            get;
            private set;
        }

        public CookieCollection ResponseCookies
        {
            get;
            set;
        }

        public Uri ResponseUri
        {
            get;
            set;
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            var request = (HttpWebRequest)base.GetWebRequest(address);
            request.CookieContainer = CookieContainer;
            return request;
        }

        protected override WebResponse GetWebResponse(WebRequest request)
        {
            var response = (HttpWebResponse)base.GetWebResponse(request);
            ResponseCookies = response.Cookies;
            ResponseUri = response.ResponseUri;
            return response;
        }
    }
}