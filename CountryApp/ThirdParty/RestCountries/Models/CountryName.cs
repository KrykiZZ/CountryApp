﻿using System.Text.Json.Serialization;

namespace CountryApp.ThirdParty.RestCountries.Models
{
    public class CountryName
    {
        [JsonPropertyName("common")]
        public string Common { get; set; }

        [JsonPropertyName("official")]
        public string Official { get; set; }
    }
}