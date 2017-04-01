using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Infrastructure.Extensions;
using Infrastructure.WebApi.Extensions;

namespace Infrastructure.Web.Security.AntiForgery
{
    public static class InfrastructureAntiForgeryManagerWebApiExtensions
    {
        public static void SetCookie(this IInfrastructureAntiForgeryManager manager, HttpResponseHeaders headers)
        {
            headers.SetCookie(new Cookie(manager.Configuration.TokenCookieName, manager.GenerateToken()));
        }

        public static bool IsValid(this IInfrastructureAntiForgeryManager manager, HttpRequestHeaders headers)
        {
            var cookieTokenValue = GetCookieValue(manager, headers);

            if (cookieTokenValue.IsNullOrEmpty())
            {
                return true;
            }

            var headerTokenValue = GetHeaderValue(manager, headers);

            if (headerTokenValue.IsNullOrEmpty())
            {
                return false;
            }
            return manager.As<IAntiForgeryValidator>().IsValid(cookieTokenValue, headerTokenValue);
        }

        private static string GetCookieValue(IInfrastructureAntiForgeryManager manager, HttpRequestHeaders headers)
        {
            var cookie = headers.GetCookies(manager.Configuration.TokenCookieName).LastOrDefault();

            if (cookie == null)
            {
                return null;
            }
            return cookie[manager.Configuration.TokenCookieName].Value;
        }

        private static string GetHeaderValue(IInfrastructureAntiForgeryManager manager, HttpRequestHeaders headers)
        {
            IEnumerable<string> headerValues;

            if (!headers.TryGetValues(manager.Configuration.TokenHeaderName, out headerValues))
            {
                return null;
            }
            var headersArray = headerValues.ToArray();

            if (!headersArray.Any())
            {
                return null;
            }
            return headersArray.Last().Split(", ").Last();
        }
    }
}
