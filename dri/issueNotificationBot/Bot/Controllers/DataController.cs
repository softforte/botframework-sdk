﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using IssueNotificationBot.Models;
using IssueNotificationBot.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Connector.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Microsoft.Rest.Serialization;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace IssueNotificationBot
{
    // This ASP Controller handles data sent from the Azure Function that polls the GitHub issues.
    // It requires that the Azure Function uses the bot AppId and AppPassword
    [ApiController]
    public class DataController : ControllerBase
    {
        private readonly IConfiguration Configuration;
        private readonly GitHubDataProcessor GitHubDataProcessor;
        private readonly ILogger Logger;
        private readonly SimpleCredentialProvider Credentials;

        public DataController(IConfiguration configuration, GitHubDataProcessor gitHubDataProcessor, ILogger<DataController> logger)
        {
            Configuration = configuration;
            GitHubDataProcessor = gitHubDataProcessor;
            Logger = logger;
            Credentials = new SimpleCredentialProvider(Configuration["MicrosoftAppId"], Configuration["MicrosoftAppPassword"]);
        }

        [HttpPost("/api/issues")]
        public async Task<HttpStatusCode> PostIssuesAsync()
        {
            Logger.LogInformation("Received post on /api/data/issues");
            using var reader = new System.IO.StreamReader(Request.Body);
            var json = await reader.ReadToEndAsync().ConfigureAwait(true);

            try
            {
                if (await IsAuthenticatedAsync(Request))
                {
                    var gitHubData = SafeJsonConvert.DeserializeObject<GitHubIssues>(json);
                    await GitHubDataProcessor.ProcessData(gitHubData);

                    Logger.LogInformation("Finished processing post on /api/data/issues");
                    return HttpStatusCode.OK;
                }

                return HttpStatusCode.Forbidden;
            }
            catch (JsonException)
            {
                Logger.LogError("Received invalid data on /api/data/issues");
            }
            catch (Exception e)
            {
                Logger.LogError($"Something went wrong in /api/data/issues controller: {e.Message}");
            }
            return HttpStatusCode.BadRequest;
        }

        [HttpPost("/api/prs")]
        public async Task<HttpStatusCode> PostPRsAsync()
        {
            Logger.LogInformation("Received post on /api/data/prs");
            using var reader = new System.IO.StreamReader(Request.Body);
            var json = await reader.ReadToEndAsync().ConfigureAwait(true);

            try
            {
                if (await IsAuthenticatedAsync(Request))
                {
                    var gitHubData = SafeJsonConvert.DeserializeObject<GitHubPRs>(json);
                    // TODO: Process Data
                    //await GitHubDataProcessor.ProcessData(gitHubData);

                    Logger.LogInformation("Finished processing post on /api/data/prs");
                    return HttpStatusCode.OK;
                }

                return HttpStatusCode.Forbidden;
            }
            catch (JsonException)
            {
                Logger.LogError("Received invalid data on /api/data/prs");
            }
            catch (Exception e)
            {
                Logger.LogError($"Something went wrong in /api/data/prs controller: {e.Message}");
            }
            return HttpStatusCode.BadRequest;
        }

        private async Task<bool> IsAuthenticatedAsync(Microsoft.AspNetCore.Http.HttpRequest request)
        {
            request.Headers.TryGetValue("Authorization", out StringValues authHeader);
            var result = await JwtTokenValidation.ValidateAuthHeader(authHeader, Credentials, new SimpleChannelProvider(), Channels.Directline);

            return result.IsAuthenticated;
        }
    }
}
