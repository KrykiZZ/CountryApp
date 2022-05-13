using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CountryApp.ThirdParty.RestCountries.Models
{
    public class Country
    {
        [JsonPropertyName("name")]
        public CountryName Name { get; set; }

        [JsonPropertyName("capital")]
        public List<string> Capital { get; set; }

        [JsonPropertyName("region")]
        public string Region { get; set; }

        [JsonPropertyName("languages")]
        public Dictionary<string, string> Languages { get; set; }
    }
}