using Android.App;
using Android.OS;
using CountryApp.ThirdParty.RestCountries.Models;
using System.Text.Json;

namespace CountryApp.Activities
{
    [Activity(Label = "CountryActivity")]
    public class CountryActivity : Activity
    {
        private Country _country;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.country_information);

            // Т.к сериализатор НЕ возвращает null - имеет смысл проверить поля на валидность
            // и вывести сообщение об ошибке, если проверка не пройдёт.
            _country = JsonSerializer.Deserialize<Country>(Intent.GetStringExtra("country"));

            FindViewById<Android.Widget.TextView>(Resource.Id.countryName).Text = _country.Name.Common;
            FindViewById<Android.Widget.TextView>(Resource.Id.countryRegion).Text = _country.Region;
            FindViewById<Android.Widget.TextView>(Resource.Id.countryCapital).Text = string.Join(", ", _country.Capital);
            FindViewById<Android.Widget.TextView>(Resource.Id.countryLanguages).Text = string.Join(", ", _country.Languages.Values);
        }
    }
}