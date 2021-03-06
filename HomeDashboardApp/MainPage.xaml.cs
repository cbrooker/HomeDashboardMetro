﻿using System.Threading.Tasks;
using HomeDashboardApp.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237
using HomeDashboardApp.Models;

namespace HomeDashboardApp
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        /// <summary>
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }


        public MainPage()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;
        }

        /// <summary>
        /// Populates the page with content passed during navigation. Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session. The state will be null the first time a page is visited.</param>
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// 
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="GridCS.Common.NavigationHelper.LoadState"/>
        /// and <see cref="GridCS.Common.NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);

            LoademoncmsValues();
            
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion



        private async void LoademoncmsValues()
        {
            //var emoncmsIds = new int[] {5, 3, 2, 10, 4, 18, 17};

            var masterBr = await MakeWebRequest("http://home.chrisbrooker.ca:83/emoncms/feed/value.json?id=5&apikey=4598c7bc07e9c7380df636265340beea");
            var natesroom = await MakeWebRequest("http://home.chrisbrooker.ca:83/emoncms/feed/value.json?id=3&apikey=4598c7bc07e9c7380df636265340beea");
            var livingroom = await MakeWebRequest("http://home.chrisbrooker.ca:83/emoncms/feed/value.json?id=2&apikey=4598c7bc07e9c7380df636265340beea");
            var office = await MakeWebRequest("http://home.chrisbrooker.ca:83/emoncms/feed/value.json?id=10&apikey=4598c7bc07e9c7380df636265340beea");
            var backroom = await MakeWebRequest("http://home.chrisbrooker.ca:83/emoncms/feed/value.json?id=4&apikey=4598c7bc07e9c7380df636265340beea");
            var outside = await MakeWebRequest("http://home.chrisbrooker.ca:83/emoncms/feed/value.json?id=18&apikey=4598c7bc07e9c7380df636265340beea");
            var humidity = await MakeWebRequest("http://home.chrisbrooker.ca:83/emoncms/feed/value.json?id=17&apikey=4598c7bc07e9c7380df636265340beea");

            var emoncmsValueCollection = new EmoncmsCollection();
            
            emoncmsValueCollection.Add(new emoncmsItem() { Name="Office", Value = String.Format("{0:0.0}c", Double.Parse(office.Replace("\"", ""))) });
            emoncmsValueCollection.Add(new emoncmsItem() { Name = "Living", Value = String.Format("{0:0.0}c", Double.Parse(livingroom.Replace("\"", ""))) });
            emoncmsValueCollection.Add(new emoncmsItem() { Name = "Backroom", Value = String.Format("{0:0.0}c", Double.Parse(backroom.Replace("\"", ""))) });
            emoncmsValueCollection.Add(new emoncmsItem() { Name = "Nate's", Value = String.Format("{0:0.0}c", Double.Parse(natesroom.Replace("\"", ""))) });
            emoncmsValueCollection.Add(new emoncmsItem() { Name = "Master", Value = String.Format("{0:0.0}c", Double.Parse(masterBr.Replace("\"", ""))) });
            emoncmsValueCollection.Add(new emoncmsItem() { Name = "Outside", Value = String.Format("{0:0.0}c", Double.Parse(outside.Replace("\"", ""))) });
            emoncmsValueCollection.Add(new emoncmsItem() { Name = "Humidity", Value = String.Format("{0:0.0}%", Double.Parse(humidity.Replace("\"", ""))) });

            var avgTemp = (Double.Parse(livingroom.Replace("\"", "")) +
                            Double.Parse(office.Replace("\"", "")) +
                            Double.Parse(backroom.Replace("\"", "")) +
                            Double.Parse(natesroom.Replace("\"", "")) +
                            Double.Parse(masterBr.Replace("\"", ""))) / 5;

            emoncmsValueCollection.Add(new emoncmsItem() { Name = "Average", Value = String.Format("{0:0.0}c", avgTemp) });

            EmoncmsGridView.ItemsSource = emoncmsValueCollection;

        }


        private async Task<string> MakeWebRequest(string url)
        {
            var http = new System.Net.Http.HttpClient();
            var response = await http.GetAsync(url);
            return await response.Content.ReadAsStringAsync();
        }
    }
}
