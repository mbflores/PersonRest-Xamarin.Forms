using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace PersonRest
{
	public partial class MainPage : ContentPage
	{
	    private const string url = "http://person20180606121813.azurewebsites.net/api/person";
	    string sContentType = "application/json";
	    private HttpClient _client = new HttpClient();
	    private ObservableCollection<Person> _person = new ObservableCollection<Person>();
		public MainPage()
		{
			InitializeComponent();
		}
        protected override async void OnAppearing()
        {
            var post = await GetPeopleAsync();           
            _person =  new ObservableCollection<Person>(post);
            LstView.ItemsSource = _person;
            base.OnAppearing();
        }

        

	    private async Task<ObservableCollection<Person>> GetPeopleAsync()
	    {
	        var result = Task.Run( async() =>
	        {
	            var content= await _client.GetStringAsync(url);
	            var post = JsonConvert.DeserializeObject<List<Person>>(content);
	            return new ObservableCollection<Person>(post);


	        });

	        return await result;



	    }

	    private async void OnAdd_Clicked(object sender, EventArgs e)
	    {
	        var person = new Person {Name = DateTime.Now.Ticks.ToString()};
	        var content = JsonConvert.SerializeObject(person);
	        _person.Insert(0, person);
	        await _client.PostAsync(url, new StringContent(content,Encoding.UTF8,sContentType));
	        
	    }

        private async void OnDelete_Clicked(object sender, EventArgs e)
        {
            var person = _person[0];
            _person.Remove(person);
            await _client.DeleteAsync(url + "/" + person.Id);
        }

        private async void OnUpdate_Clicked(object sender, EventArgs e)
        {
            var person = _person[0];
            person.Name += "Update";
            var content = JsonConvert.SerializeObject(person);            
            await _client.PutAsync(url+"/" + person.Id, new StringContent(content,Encoding.UTF8,sContentType));
        }

        private async void LstView_Refreshing(object sender, EventArgs e)
        {
            var post = await GetPeopleAsync();           
            _person =  new ObservableCollection<Person>(post);
            LstView.ItemsSource = _person;
            LstView.EndRefresh();
        }

        private async void Delete_Clicked(object sender, EventArgs e)
        {
            var selected = (sender as MenuItem).CommandParameter as Person;
            _person.Remove(selected);
            await _client.DeleteAsync(url + "/" + selected.Id);
            
        }

        private async void Update_Clicked(object sender, EventArgs e)
        {


            var person = (sender as MenuItem).CommandParameter as Person;
            person.Name += "Update";
            var content = JsonConvert.SerializeObject(person);            
            await _client.PutAsync(url+"/" + person.Id, new StringContent(content,Encoding.UTF8,sContentType));

        }

        private void LstView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var selected = e.SelectedItem as Person;
            DisplayAlert("ID", selected.Id.ToString(), "OK");
        }
    }
}
