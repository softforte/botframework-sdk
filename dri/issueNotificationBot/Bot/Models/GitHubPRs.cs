﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Newtonsoft.Json;
using System;
using System.Collections.Generic;

#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.

#nullable enable
namespace IssueNotificationBot.Models
{
    public class GitHubPRs
    {
        [JsonProperty(PropertyName = "singleReviewers")]
        public readonly Dictionary<string, GitHubPR[]> SingleReviewers;
        [JsonProperty(PropertyName = "groupReviewers")]
        public readonly Dictionary<string, GitHubPR[]> GroupReviewers;
    }

    public class GitHubPR
    {
        [JsonProperty(PropertyName = "assignees")]
        public readonly GitHubBasicUserInfo[] Assignees;
        [JsonProperty(PropertyName = "author")]
        public readonly GitHubBasicUserInfo Author;
        [JsonProperty(PropertyName = "body")]
        public readonly string Body;
        [JsonProperty(PropertyName = "createdAt")]
        public readonly DateTime CreatedAt;
        [JsonProperty(PropertyName = "isDraft")]
        public readonly bool IsDraft;
        [JsonProperty(PropertyName = "labels")]
        public readonly GitHubLabel[] Labels;
        [JsonProperty(PropertyName = "mergeable")]
        public readonly string Mergeable;
        [JsonProperty(PropertyName = "number")]
        public readonly int Number;
        [JsonProperty(PropertyName = "repository")]
        public readonly GitHubRepository Repository;
        [JsonProperty(PropertyName = "reviewDecision")]
        public readonly string ReviewDecision;
        [JsonProperty(PropertyName = "reviewingForGroup")]
        public readonly string? ReviewingForGroup;
        [JsonProperty(PropertyName = "reviewRequests")]
        public readonly GitHubReviewRequest[] ReviewRequests;
        [JsonProperty(PropertyName = "reviews")]
        public readonly GitHubReview[] Reviews;
        [JsonProperty(PropertyName = "state")]
        public readonly string State;
        [JsonProperty(PropertyName = "title")]
        public readonly string Title;
        [JsonProperty(PropertyName = "url")]
        public readonly Uri Url;
    }

    public class GitHubReviewRequest
    {
        [JsonProperty(PropertyName = "requestedReviewer")]
        public readonly GitHubRequestedReviewer RequestedReviewer;
    }

    public class GitHubRequestedReviewer : GitHubBasicUserInfo
    {
        [JsonProperty(PropertyName = "members")]
        public readonly GitHubBasicUserInfo[]? Members;
    }

    public class GitHubReview
    {
        [JsonProperty(PropertyName = "author")]
        public readonly GitHubBasicUserInfo Author;
        [JsonProperty(PropertyName = "state")]
        public readonly string State;
    }
}
