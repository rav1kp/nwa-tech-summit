﻿using Newtonsoft.Json;
using System.Collections.Generic;

namespace Smoker
{
    public class Request
    {
        public int SortOrder = 100;
        public bool IsBaseTest = false;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? Index;
        public string Verb = "GET";
        public string Url = string.Empty;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ContentType = null;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Body = null;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<Header> Headers = null;
        public Validation Validation = new Validation();
    }

    public class Header
    {
        public string Key;
        public string Value;
    }

    public class Validation
    {
        public int Code = 200;
        public string ContentType = "application/json";
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? MinLength;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? MaxLength;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? MaxMilliseconds;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<Contain> Contains = new List<Contain>();

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Json Json;
    }

    public class Contain
    {
        public string Value = string.Empty;
        public bool IsCaseSensitive = true;
    }

    public class Json
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? MinLength;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? MaxLength;
    }
}
