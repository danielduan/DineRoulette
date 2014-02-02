using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using DineRoulette.Resources;

using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using System.IO.IsolatedStorage;

using Newtonsoft.Json;
using System.Windows.Media.Imaging;
using Microsoft.Phone.Tasks;

namespace DineRoulette
{
    public class Location
    {
        public string cross_streets { get; set; }
        public string city { get; set; }
        public List<string> display_address { get; set; }
        public string postal_code { get; set; }
        public string country_code { get; set; }
        public List<string> address { get; set; }
        public string state_code { get; set; }
    }

    public class Restaurant
    {
        public bool is_claimed { get; set; }
        public double distance { get; set; }
        public string mobile_url { get; set; }
        public string rating_img_url { get; set; }
        public int review_count { get; set; }
        public string name { get; set; }
        public string snippet_image_url { get; set; }
        public double rating { get; set; }
        public string url { get; set; }
        public Location location { get; set; }
        public int menu_date_updated { get; set; }
        public string phone { get; set; }
        public string snippet_text { get; set; }
        public string image_url { get; set; }
        public List<List<string>> categories { get; set; }
        public string display_phone { get; set; }
        public string rating_img_url_large { get; set; }
        public string menu_provider { get; set; }
        public string id { get; set; }
        public bool is_closed { get; set; }
        public string rating_img_url_small { get; set; }
    }

    public partial class MainPage : PhoneApplicationPage
    {
        Restaurant currentRest;
        // Constructor
        public MainPage()
        {
            InitializeComponent();

        }

        private async void GetRestarants() {
            var url = "http://dineroulette.herokuapp.com/mobile/v1/getrestaurants";

            Geolocator geolocator = new Geolocator();
            //geolocator.DesiredAccuracyInMeters = 50;

            try
            {
                Geoposition geoposition = await geolocator.GetGeopositionAsync(
                    maximumAge: TimeSpan.FromMinutes(5),
                    timeout: TimeSpan.FromSeconds(10)
                    );

                var location = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("lat", geoposition.Coordinate.Latitude.ToString("0.00")),
                    new KeyValuePair<string, string>("lon", geoposition.Coordinate.Longitude.ToString("0.00"))
                };

                var httpClient = new HttpClient(new HttpClientHandler());
                HttpResponseMessage response = await httpClient.PostAsync(url, new FormUrlEncodedContent(location));
                response.EnsureSuccessStatusCode();
                var responseString = await response.Content.ReadAsStringAsync();

                //dynamic restaurant = JsonConvert.DeserializeObject(responseString);

                var restaurant = JsonConvert.DeserializeObject<Restaurant>(responseString);

                InfoGrid.Visibility = Visibility.Visible;

                progressPage.Visibility = Visibility.Collapsed;
                ModalGrid.Visibility = Visibility.Collapsed;
                NameTextBlock.Text = restaurant.name;
                string address = "";
                foreach (var key in restaurant.location.address)
                {
                    address += key + " ";
                }
                address += restaurant.location.city + ", ";
                address += restaurant.location.state_code;
                
                AddressTextBlock.Text = address;
                TaglineTextBlock.Text = restaurant.snippet_text;
                PhoneTextBlock.Text = restaurant.phone;

                BitmapImage rest_img = new BitmapImage(new Uri(restaurant.image_url, UriKind.Absolute));
                RestaurantImage.Source = rest_img;
                currentRest = restaurant;
            }
            catch (Exception e)
            {
                MessageBox.Show("Exception caught, please check your internet connection.");
                progressPage.Visibility = Visibility.Collapsed;
                ModalGrid.Visibility = Visibility.Collapsed;
            }

        }

        private void FindButton_Click(object sender, RoutedEventArgs e)
        {
            progressPage.Visibility = System.Windows.Visibility.Visible;
            ModalGrid.Visibility = System.Windows.Visibility.Visible;
            GetRestarants();
        }


    }
}