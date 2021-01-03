﻿using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Client.WorkerService.Client
{
    public abstract class SwaggerClientBase
    {
        public string BearerToken { get; private set; }

        public void SetBearerToken(string token)
        {
            BearerToken = token;
        }

        // Called by implementing swagger client classes
        protected Task<HttpRequestMessage> CreateHttpRequestMessageAsync(CancellationToken cancellationToken)
        {
            var msg = new HttpRequestMessage();
            // SET THE BEARER AUTH TOKEN
            msg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", BearerToken);
            return Task.FromResult(msg);
        }
    }
}
