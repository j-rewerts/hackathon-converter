using Google.Apis.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Converter.Services.TaskRunner
{
    public class GoogleAuthenticator : IConfigurableHttpClientInitializer
    {

        public void Initialize(ConfigurableHttpClient httpClient)
        {
            // TODO: add the OAuth token to the httpClient
            throw new NotImplementedException();
        }
    }
}
