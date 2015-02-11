using System.Threading.Tasks;
using HomeDashboardApp.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Hub Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=321224
using HomeDashboardApp.Models;
using WinRTXamlToolkit.Controls.Extensions;

namespace HomeDashboardApp
{

    /// <summary>
    /// A page that displays a grouped collection of items.
    /// </summary>
    public sealed partial class HubPage1 : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        private string _masterTemp;
        private string _natesTemp;
        private string _livingTemp;
        private string _officeTemp;
        private string _backroomTemp;
        private string _outsideTemp;
        private string _humidity;
        private string _averageTemp;


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

        public HubPage1()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
        }


        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            // TODO: Assign a collection of bindable groups to this.DefaultViewModel["Groups"]

        }


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

            var tb = FindChildControl<TextBlock>(HeroSection, "MasterTempTextBlock") as TextBlock;
            if (tb != null) tb.Text = String.Format("{0:0.0}c", Double.Parse(office.Replace("\"", "")));

            tb = FindChildControl<TextBlock>(HeroSection, "NatesTempTextBlock") as TextBlock;
            if (tb != null) tb.Text = String.Format("{0:0.0}c", Double.Parse(office.Replace("\"", "")));

            //emoncmsValueCollection.Add(new emoncmsItem() { Name = "Office", Value = String.Format("{0:0.0}c", Double.Parse(office.Replace("\"", ""))) });
            //emoncmsValueCollection.Add(new emoncmsItem() { Name = "Living", Value = String.Format("{0:0.0}c", Double.Parse(livingroom.Replace("\"", ""))) });
            //emoncmsValueCollection.Add(new emoncmsItem() { Name = "Backroom", Value = String.Format("{0:0.0}c", Double.Parse(backroom.Replace("\"", ""))) });
            //emoncmsValueCollection.Add(new emoncmsItem() { Name = "Nate's", Value = String.Format("{0:0.0}c", Double.Parse(natesroom.Replace("\"", ""))) });
            //emoncmsValueCollection.Add(new emoncmsItem() { Name = "Master", Value = String.Format("{0:0.0}c", Double.Parse(masterBr.Replace("\"", ""))) });
            //emoncmsValueCollection.Add(new emoncmsItem() { Name = "Outside", Value = String.Format("{0:0.0}c", Double.Parse(outside.Replace("\"", ""))) });
            //emoncmsValueCollection.Add(new emoncmsItem() { Name = "Humidity", Value = String.Format("{0:0.0}%", Double.Parse(humidity.Replace("\"", ""))) });

            //var avgTemp = (Double.Parse(livingroom.Replace("\"", "")) +
            //                Double.Parse(office.Replace("\"", "")) +
            //                Double.Parse(backroom.Replace("\"", "")) +
            //                Double.Parse(natesroom.Replace("\"", "")) +
            //                Double.Parse(masterBr.Replace("\"", ""))) / 5;

            //emoncmsValueCollection.Add(new emoncmsItem() { Name = "Average", Value = String.Format("{0:0.0}c", avgTemp) });

            

        }


        private async Task<string> MakeWebRequest(string url)
        {
            var http = new System.Net.Http.HttpClient();
            var response = await http.GetAsync(url);
            return await response.Content.ReadAsStringAsync();
        }
        private DependencyObject FindChildControl<T>(DependencyObject control, string ctrlName)
        {
            int childNumber = VisualTreeHelper.GetChildrenCount(control);
            for (int i = 0; i < childNumber; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(control, i);
                FrameworkElement fe = child as FrameworkElement;
                // Not a framework element or is null
                if (fe == null) return null;

                if (child is T && fe.Name == ctrlName)
                {
                    // Found the control so return
                    return child;
                }
                else
                {
                    // Not found it - search children
                    DependencyObject nextLevel = FindChildControl<T>(child, ctrlName);
                    if (nextLevel != null)
                        return nextLevel;
                }
            }
            return null;
        }


        private async void MasterTempTextBlockLoaded(object sender, RoutedEventArgs e)
        {
            var masterBr = await MakeWebRequest("http://home.chrisbrooker.ca:83/emoncms/feed/value.json?id=5&apikey=4598c7bc07e9c7380df636265340beea");
            _masterTemp = String.Format("{0:0.0}c", Double.Parse(masterBr.Replace("\"", "")));
            ((TextBlock)sender).Text = _masterTemp;
        }
        private async void BackroomTempTextBlockLoaded(object sender, RoutedEventArgs e)
        {
            var backroom = await MakeWebRequest("http://home.chrisbrooker.ca:83/emoncms/feed/value.json?id=4&apikey=4598c7bc07e9c7380df636265340beea");
            _backroomTemp = String.Format("{0:0.0}c", Double.Parse(backroom.Replace("\"", "")));
            ((TextBlock)sender).Text = _backroomTemp;
        }
        private async void OfficeTempTextBlockLoaded(object sender, RoutedEventArgs e)
        {
            var office = await MakeWebRequest("http://home.chrisbrooker.ca:83/emoncms/feed/value.json?id=10&apikey=4598c7bc07e9c7380df636265340beea");
            _officeTemp = String.Format("{0:0.0}c", Double.Parse(office.Replace("\"", "")));
            ((TextBlock)sender).Text = _officeTemp;
        }

        private async void LivingTempTextBlockLoaded(object sender, RoutedEventArgs e)
        {
            var livingroom = await MakeWebRequest("http://home.chrisbrooker.ca:83/emoncms/feed/value.json?id=2&apikey=4598c7bc07e9c7380df636265340beea");
            _livingTemp = String.Format("{0:0.0}c", Double.Parse(livingroom.Replace("\"", "")));
            ((TextBlock)sender).Text = _livingTemp;
        }

        private async void NateTempTextBlockLoaded(object sender, RoutedEventArgs e)
        {
            var natesroom = await MakeWebRequest("http://home.chrisbrooker.ca:83/emoncms/feed/value.json?id=3&apikey=4598c7bc07e9c7380df636265340beea");
            _natesTemp = String.Format("{0:0.0}c", Double.Parse(natesroom.Replace("\"", "")));
            ((TextBlock)sender).Text = _natesTemp;
        }

        private async void OutsideTempTextBlock_OnLoadedTempTextBlockLoaded(object sender, RoutedEventArgs e)
        {
            var outside = await MakeWebRequest("http://home.chrisbrooker.ca:83/emoncms/feed/value.json?id=18&apikey=4598c7bc07e9c7380df636265340beea");
            _outsideTemp = String.Format("{0:0.0}c", Double.Parse(outside.Replace("\"", "")));
            ((TextBlock)sender).Text = _outsideTemp;
        }

        private async void AverageTempTextBlock_OnLoadedTempTextBlockLoaded(object sender, RoutedEventArgs e)
        {
            
        }

        private async void HumidityTextBlock_OnLoadedTextBlockLoaded(object sender, RoutedEventArgs e)
        {
            var humidity = await MakeWebRequest("http://home.chrisbrooker.ca:83/emoncms/feed/value.json?id=17&apikey=4598c7bc07e9c7380df636265340beea");
            _humidity = String.Format("{0:0.0}%", Double.Parse(humidity.Replace("\"", "")));
            ((TextBlock)sender).Text = _humidity;
        }

        private async void UpdateEmoncmsValues()
        {
            var masterBr = await MakeWebRequest("http://home.chrisbrooker.ca:83/emoncms/feed/value.json?id=5&apikey=4598c7bc07e9c7380df636265340beea");
            var natesroom = await MakeWebRequest("http://home.chrisbrooker.ca:83/emoncms/feed/value.json?id=3&apikey=4598c7bc07e9c7380df636265340beea");
            var livingroom = await MakeWebRequest("http://home.chrisbrooker.ca:83/emoncms/feed/value.json?id=2&apikey=4598c7bc07e9c7380df636265340beea");
            var office = await MakeWebRequest("http://home.chrisbrooker.ca:83/emoncms/feed/value.json?id=10&apikey=4598c7bc07e9c7380df636265340beea");
            var backroom = await MakeWebRequest("http://home.chrisbrooker.ca:83/emoncms/feed/value.json?id=4&apikey=4598c7bc07e9c7380df636265340beea");
            var outside = await MakeWebRequest("http://home.chrisbrooker.ca:83/emoncms/feed/value.json?id=18&apikey=4598c7bc07e9c7380df636265340beea");
            var humidity = await MakeWebRequest("http://home.chrisbrooker.ca:83/emoncms/feed/value.json?id=17&apikey=4598c7bc07e9c7380df636265340beea");

            _masterTemp = String.Format("{0:0.0}c", Double.Parse(masterBr.Replace("\"", "")));
            _natesTemp = String.Format("{0:0.0}c", Double.Parse(natesroom.Replace("\"", "")));
            _livingTemp = String.Format("{0:0.0}c", Double.Parse(livingroom.Replace("\"", "")));
            _officeTemp = String.Format("{0:0.0}c", Double.Parse(office.Replace("\"", "")));
            _backroomTemp = String.Format("{0:0.0}c", Double.Parse(backroom.Replace("\"", "")));
            _outsideTemp = String.Format("{0:0.0}c", Double.Parse(outside.Replace("\"", "")));
            _humidity = String.Format("{0:0.0}%", Double.Parse(humidity.Replace("\"", "")));
            
            var avgTemp = (Double.Parse(livingroom.Replace("\"", "")) +
                            Double.Parse(office.Replace("\"", "")) +
                            Double.Parse(backroom.Replace("\"", "")) +
                            Double.Parse(natesroom.Replace("\"", "")) +
                            Double.Parse(masterBr.Replace("\"", ""))) / 5;

            _averageTemp = String.Format("{0:0.0}c", avgTemp);

            UpdateEmoncmsTextBlocks();
        }

        private void UpdateEmoncmsTextBlocks()
        {
            var tb = FindChildControl<TextBlock>(HeroSection, "MasterTempTextBlock") as TextBlock;
            if (tb != null) tb.Text = _masterTemp;

            tb = FindChildControl<TextBlock>(HeroSection, "NatesTempTextBlock") as TextBlock;
            if (tb != null) tb.Text = _natesTemp;

            tb = FindChildControl<TextBlock>(HeroSection, "LivingTempTextBlock") as TextBlock;
            if (tb != null) tb.Text = _livingTemp;

            tb = FindChildControl<TextBlock>(HeroSection, "OfficeTempTextBlock") as TextBlock;
            if (tb != null) tb.Text = _officeTemp;

            tb = FindChildControl<TextBlock>(HeroSection, "BackroomTempTextBlock") as TextBlock;
            if (tb != null) tb.Text = _backroomTemp;

            tb = FindChildControl<TextBlock>(HeroSection, "OutsideTempTextBlock") as TextBlock;
            if (tb != null) tb.Text = _outsideTemp;

            tb = FindChildControl<TextBlock>(HeroSection, "AverageTempTextBlock") as TextBlock;
            if (tb != null) tb.Text = _averageTemp;

            tb = FindChildControl<TextBlock>(HeroSection, "HumidityTextBlock") as TextBlock;
            if (tb != null) tb.Text = _humidity;
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

            var timer = new DispatcherTimer { Interval = TimeSpan.FromMinutes(1) };
            timer.Tick += TimerTicker;
            timer.Start();
            
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private void TimerTicker(object sender, object e)
        {
            UpdateEmoncmsValues();
            
        }


        
    }
}
