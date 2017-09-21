using Google.Apis.Http;
using System;
using System.Collections.Generic;
using System.Text;

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
            httpClient.DefaultRequestHeaders.Add("Bearer", _oauthToken);
        }
    }
}
