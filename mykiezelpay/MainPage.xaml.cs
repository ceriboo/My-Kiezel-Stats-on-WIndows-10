using datakiezel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Pour plus d'informations sur le modèle d'élément Page vierge, consultez la page https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace mykiezelpay
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        
        string url = "https://api.kiezelpay.com/api/merchant/today?offset={0}&key=";
        string urlyesterday = "https://api.kiezelpay.com/api/merchant/yesterday?offset={0}&key=";
        string urlapps = "";
        DispatcherTimer period = new DispatcherTimer();
        mydata resultat2;
        int lachat;
        public MainPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode =
                                        Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
            period.Tick += Period_Tick;
            period.Interval = new TimeSpan(0, 5, 0);
            recupaujourdhui("fitbit");
            recupaujourdhui("pebble");
            recupaujourdhui("garmin");
            recupaujourdhui("");
            updatelistapps();
            summary();
            period.Start();
            var bounds = Window.Current.Bounds;
            double height = bounds.Height;
            Debug.WriteLine(height);
            double width = bounds.Width;
            Debug.WriteLine(width);
        }

        private void Period_Tick(object sender, object e)
        {
            Debug.WriteLine("tick");
            recupaujourdhui("fitbit");
            recupaujourdhui("pebble");
            recupaujourdhui("garmin");
            recupaujourdhui("");
            updatelistapps();
            summary();
        }


        private void refresh_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("tick");
            recupaujourdhui("fitbit");
            recupaujourdhui("pebble");
            recupaujourdhui("garmin");
            recupaujourdhui("");
            summary();
        }

        async void recupaujourdhui(string platform)
        {
            int utc = Math.Abs((DateTimeKind.Local - DateTimeKind.Utc) * 60);
            string _url = String.Format(url, utc.ToString()) + App.Apikey;
            string _url2 = String.Format(urlyesterday, utc.ToString()) + App.Apikey;
            if (platform != "")
            {
                _url += "&platform=" + platform;
                _url2 += "&platform=" + platform;
            }

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36");

            //HttpClient client = new HttpClient();
            HttpResponseMessage laresponse = new HttpResponseMessage();
            HttpResponseMessage laresponse2 = new HttpResponseMessage();
            //laresponse.Version = new Version("1.1");
            //laresponse.RequestMessage.Version = new Version("1.1");
            laresponse = await client.GetAsync(_url);
            laresponse2 = await client.GetAsync(_url2);

            if (laresponse.IsSuccessStatusCode && laresponse2.IsSuccessStatusCode)
            {
                var toto = await laresponse.Content.ReadAsStringAsync();
                var toto2 = await laresponse2.Content.ReadAsStringAsync();
                Console.WriteLine(toto);
                toto = "{\"datakiezel\":" + toto.ToString() + "}";
                toto2 = "{\"datakiezel\":" + toto2.ToString() + "}";


                mydata resultat = JsonConvert.DeserializeObject<mydata>(toto);
                resultat2 = JsonConvert.DeserializeObject<mydata>(toto2);

                switch (platform)
                {
                    case "fitbit":
                        // purchase
                        purchasesfitbit.Text = resultat.datakiezel.purchases.ToString();
                        purchasesfitbit2.Foreground = new SolidColorBrush(colorarrow(resultat.datakiezel.purchases, resultat2.datakiezel.purchases));
                        purchasesfitbit2.Text = positionarrow(resultat.datakiezel.purchases, resultat2.datakiezel.purchases);
                        //amount 
                        amountfitbit.Text = resultat.datakiezel.amount.ToString() + "$";
                        amountfitbit2.Foreground = new SolidColorBrush(colorarrow(resultat.datakiezel.amount, resultat2.datakiezel.amount));
                        amountfitbit2.Text = positionarrow(resultat.datakiezel.amount, resultat2.datakiezel.amount);

                        //rank
                        rankfitbit.Text = resultat.datakiezel.rank.ToString();
                        rankfitbit2.Foreground = new SolidColorBrush(colorarrow(resultat.datakiezel.rank, resultat2.datakiezel.rank));
                        rankfitbit2.Text = positionarrow(resultat.datakiezel.rank, resultat2.datakiezel.rank);

                        break;
                    case "garmin":
                        //purchase
                        purchasesgarmin.Text = resultat.datakiezel.purchases.ToString();
                        purchasesgarmin2.Foreground = new SolidColorBrush(colorarrow(resultat.datakiezel.purchases, resultat2.datakiezel.purchases));
                        purchasesgarmin2.Text = positionarrow(resultat.datakiezel.purchases, resultat2.datakiezel.purchases);

                        //amount
                        amountgarmin.Text = resultat.datakiezel.amount.ToString() + "$";
                        amountgarmin2.Foreground = new SolidColorBrush(colorarrow(resultat.datakiezel.amount, resultat2.datakiezel.amount));
                        amountgarmin2.Text = positionarrow(resultat.datakiezel.amount, resultat2.datakiezel.amount);

                        //rank
                        rankgarmin.Text = resultat.datakiezel.rank.ToString();
                        rankgarmin2.Foreground = new SolidColorBrush(colorarrow(resultat.datakiezel.rank, resultat2.datakiezel.rank));
                        rankgarmin2.Text = positionarrow(resultat.datakiezel.rank, resultat2.datakiezel.rank);

                        break;
                    case "pebble":
                        //purchase
                        purchasespebble.Text = resultat.datakiezel.purchases.ToString();
                        purchasespebble2.Foreground = new SolidColorBrush(colorarrow(resultat.datakiezel.purchases, resultat2.datakiezel.purchases));
                        purchasespebble2.Text = positionarrow(resultat.datakiezel.purchases, resultat2.datakiezel.purchases);

                        //amount
                        amountpebble.Text = resultat.datakiezel.amount.ToString() + "$";
                        amountpebble2.Foreground = new SolidColorBrush(colorarrow(resultat.datakiezel.amount, resultat2.datakiezel.amount));
                        amountpebble2.Text = positionarrow(resultat.datakiezel.amount, resultat2.datakiezel.amount);

                        //rank
                        rankpebble.Text = resultat.datakiezel.rank.ToString();
                        rankpebble2.Foreground = new SolidColorBrush(colorarrow(resultat.datakiezel.rank, resultat2.datakiezel.rank));
                        rankpebble2.Text = positionarrow(resultat.datakiezel.rank, resultat2.datakiezel.rank);

                        break;
                    default:
                        //purchase
                        purchasestotal.Text = resultat.datakiezel.purchases.ToString();
                        purchasestotal2.Foreground = new SolidColorBrush(colorarrow(resultat.datakiezel.purchases, resultat2.datakiezel.purchases));
                        purchasestotal2.Text = positionarrow(resultat.datakiezel.purchases, resultat2.datakiezel.purchases);

                        //amount
                        amounttotal.Text = resultat.datakiezel.amount.ToString() + "$";
                        amounttotal2.Foreground = new SolidColorBrush(colorarrow(resultat.datakiezel.amount, resultat2.datakiezel.amount));
                        amounttotal2.Text = positionarrow(resultat.datakiezel.amount, resultat2.datakiezel.amount);
                        //rank
                        ranktotal.Text = resultat.datakiezel.rank.ToString();
                        ranktotal2.Foreground = new SolidColorBrush(colorarrow(resultat.datakiezel.rank, resultat2.datakiezel.rank));

                        ranktotal2.Text = positionarrow(resultat.datakiezel.rank, resultat2.datakiezel.rank);
                        if (resultat.datakiezel.purchases == 0) lachat = 0;
                        if (resultat.datakiezel.purchases > lachat)
                        {
                            ihmviens.Begin();
                            lachat = resultat.datakiezel.purchases;
                        }
                        break;

                }
            }
            else
            {
              //  error("cannot retrieve data");
            }
        }

        private void ihmviens_Completed(object sender, object e)
        {
            Task.Delay(5000);
            ihmparss.Begin();
        }

        Color colorarrow(double val1, double val2)
        {
            if (val1 > val2)
            {
                return Colors.Green;
            }
            else if (val1 < val2)
            {
                return Colors.Red;
            }
            else
            {
                return Colors.White;
            }



        }

        string positionarrow(double val1, double val2)
        {

            if (val1 > val2)
            {
                return "(+)";
            }
            else if (val1 < val2)
            {
                return "(-)";
            }
            else
            {
                return "(=)";
            }
        }
        async void CheckAppVersion()
        {


            if (Windows.Storage.ApplicationData.Current.LocalSettings.Values["AppVersion"] != null)
            {
                if (Windows.Storage.ApplicationData.Current.LocalSettings.Values["AppVersion"].ToString() != App.appVersion)
                {
                    // Our app has been updated
                    Windows.Storage.ApplicationData.Current.LocalSettings.Values["AppVersion"] = App.appVersion;

                    // Call RemoveAccess
                    BackgroundExecutionManager.RemoveAccess();
                    BackgroundAccessStatus status = await BackgroundExecutionManager.RequestAccessAsync();
                }
            }
            else
            {
                Windows.Storage.ApplicationData.Current.LocalSettings.Values["AppVersion"] = App.appVersion;

            }


        }
        async void error(string message)
        {
            try
            {
                MessageDialog erreur = new MessageDialog(message, "My kiezel stats");
                await erreur.ShowAsync();
            }
            catch { }



        }


        /// <summary>
        /// Invoqué lorsque cette page est sur le point d'être affichée dans un frame.
        /// </summary>
        /// <param name="e">Données d’événement décrivant la manière dont l’utilisateur a accédé à cette page.
        /// Ce paramètre est généralement utilisé pour configurer la page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {


            if (!App.isHardwareButtonsAPIPresent)
            {

                this.BottomAppBar.ClosedDisplayMode = AppBarClosedDisplayMode.Compact;
            }
            else
            {

            }
        }

        private void loption_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(option));
        }

        private async void politique_Click(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("http://ceriboowp.fr/airidf/airidf.html"));
        }

        private void lapropos_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(apropos));
        }

        async void updatelistapps()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36");

            //HttpClient client = new HttpClient();
            HttpResponseMessage laresponse = new HttpResponseMessage();
            laresponse = await client.GetAsync("https://api.kiezelpay.com/api/merchant/productList?key=" + App.Apikey);

            if (laresponse.IsSuccessStatusCode)
            {
                var toto = await laresponse.Content.ReadAsStringAsync();
               
                Debug.WriteLine(toto);
                toto =  toto.ToString();
              
                 App.mylist = JsonConvert.DeserializeObject<dataapps>(toto);
                listviewapps.ItemsSource = App.mylist.products;
             }

        }

        async void summary()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36");

            //HttpClient client = new HttpClient();
            HttpResponseMessage laresponse = new HttpResponseMessage();
            laresponse = await client.GetAsync("https://api.kiezelpay.com/api/merchant/summary?key=" + App.Apikey);

            if (laresponse.IsSuccessStatusCode)
            {
                var toto = await laresponse.Content.ReadAsStringAsync();

                
                toto = toto.ToString();

                var summary = JsonConvert.DeserializeObject<datasummary>(toto);

                Debug.WriteLine(summary.totalPurchases);
                _totalpurchase.Text = String.Format("total purchases:{0}. Current balance: {1}$ ",summary.totalPurchases.ToString(),summary.currentBalance.ToString());
                _lastupdate.Text = String.Format("last update at: {0}", DateTime.Now.ToString("HH:mm"));
            }

        }

        private void Listviewapps_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var appchoisie = (sender as ListView).SelectedItem as Product;

            var a = 0;
        }
    }
}
