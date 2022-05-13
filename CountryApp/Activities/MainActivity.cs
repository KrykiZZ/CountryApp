using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.Widget;
using AndroidX.AppCompat.App;
using CountryApp.ThirdParty.RestCountries;
using System.Collections.Generic;
using CountryApp.ThirdParty.RestCountries.Models;
using System.Linq;
using Android.Content;
using System.Text.Json;

namespace CountryApp.Activities
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private IEnumerable<Country> _countries;

        private Android.Widget.ListView _countriesWidget;
        private Android.Widget.ArrayAdapter _adapter;

        private SearchView _searchView;

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            Toolbar toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            _countriesWidget = FindViewById<Android.Widget.ListView>(Resource.Id.countries);

            // В реальных условиях стоит передать вызов метода фоновому обработчику
            // в то время, как клиенту будет отображён дисплей состояния/загрузки.
            _countries = await RestCountriesAPI.Instance.GetAll("name,capital,languages,region");
            _countries = _countries.OrderBy(x => x.Name.Common);

            string[] names = _countries.Select(x => x.Name.Common).ToArray();

            // Не обнаружил возможности привязать список стран при помощи биндингов (как это реализовано в Xamarin.Forms)
            // по этому используем заготовленный массив из отсортированных названий стран.
            _adapter = new Android.Widget.ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, names);
            _countriesWidget.Adapter = _adapter;
            _countriesWidget.ItemClick += Countries_ItemClick;

            _searchView = FindViewById<SearchView>(Resource.Id.search_view);
            _searchView.QueryTextChange += Search_QueryTextChange;
        }

        private void Search_QueryTextChange(object sender, SearchView.QueryTextChangeEventArgs e)
        {

        }

        private void Countries_ItemClick(object sender, Android.Widget.AdapterView.ItemClickEventArgs e)
        {
            // Следует показать сообщение об ошибки и сформировать лог для телеметрии.
            if (_countries.Count() <= e.Position)
                return;

            Intent intent = new Intent(this, typeof(CountryActivity));
            intent.PutExtra("country", JsonSerializer.Serialize(_countries.ToList()[e.Position]));

            StartActivity(intent);
        }

        protected override void OnDestroy()
        {
            // Отписываемся от события при выгрузке Activity, чтобы избежать утечки памяти
            // ввиду использования в системе нескольких GC.
            _countriesWidget.ItemClick -= Countries_ItemClick;
            _searchView.QueryTextChange -= Search_QueryTextChange;
            base.OnDestroy();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.search_button)
                return true;

            return base.OnOptionsItemSelected(item);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
	}
}
