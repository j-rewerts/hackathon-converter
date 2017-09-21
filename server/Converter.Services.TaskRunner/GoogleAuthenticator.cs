using Google.Apis.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Converter.Services.TaskRunner
{
    public class GoogleAuthenticator : IConfigurableHttpClientInitializer
    {
        public GoogleAuthenticator(string oauthToken)
        {
            if (string.IsNullOrWhiteSpace(oauthToken))
                throw new ArgumentNullException("oauthToken");
            _oauthToken = oauthToken;
        }

        private readonly string _oauthToken;

        public void Initialize(ConfigurableHttpClient httpClient)
        {

            httpClient.MessageHandler.AddExecuteInterceptor(new GoogleHttpExecuteInterceptor(_oauthToken));
        }

        private class GoogleHttpExecuteInterceptor : IHttpExecuteInterceptor
        {
            public GoogleHttpExecuteInterceptor(string oauthToken)
            {
                if (string.IsNullOrWhiteSpace(oauthToken))
                    throw new ArgumentNullException("oauthToken");
                _oauthToken = oauthToken;
            }
            private readonly string _oauthToken;

            public Task InterceptAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                request.Headers.Add("Authorization", $"Bearer { _oauthToken }");
                return Task.CompletedTask;
            }
        }
    }
}
