using CountryApp.ThirdParty.RestCountries.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace CountryApp.ThirdParty.RestCountries
{
    internal class RestCountriesAPI
    {
        // Базовая реализация ленивой инициализации.
        private static RestCountriesAPI _instance;
        public static RestCountriesAPI Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new RestCountriesAPI();
                return _instance;
            }
        }

        private HttpClient _http;

        public RestCountriesAPI()
        {
            _http = new HttpClient()
            {
                BaseAddress = new Uri("https://restcountries.com/v3.1/")
            };
        }

        public async Task<List<Country>> GetAll(string fields)
        {
            return await _http.GetFromJsonAsync<List<Country>>($"all?fields={fields}");
        }
    }
}