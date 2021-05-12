using datakiezel;
using Microsoft.QueryStringDotNET;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace mykiezelpay
{
    /// <summary>
    /// Fournit un comportement spécifique à l'application afin de compléter la classe Application par défaut.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Initialise l'objet d'application de singleton.  Il s'agit de la première ligne du code créé
        /// à être exécutée. Elle correspond donc à l'équivalent logique de main() ou WinMain().
        /// </summary>
        /// 
        public static string Apikey = "0123456789abcdef0123456789abcdef";
        public static Windows.Storage.ApplicationDataContainer settings = Windows.Storage.ApplicationData.Current.LocalSettings;
        public static dataapps mylist;
        public static String appVersion = String.Format("{0}.{1}.{2}.{3}",
            
               Package.Current.Id.Version.Major,
               Package.Current.Id.Version.Minor,
                Package.Current.Id.Version.Build,
               Package.Current.Id.Version.Revision);
        public static string lienurl;
        public static bool _taskRegistered;
        public static bool isHardwareButtonsAPIPresent =
  Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons");
       
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
            
        }

        
        /// <summary>
        /// Invoqué lorsque l'application est lancée normalement par l'utilisateur final.  D'autres points d'entrée
        /// seront utilisés par exemple au moment du lancement de l'application pour l'ouverture d'un fichier spécifique.
        /// </summary>
        /// <param name="e">Détails concernant la requête et le processus de lancement.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif

            Type lapage = typeof(MainPage);
            if (e.Kind == ActivationKind.Protocol)
            {
                LaunchActivatedEventArgs _eventArgs = e as LaunchActivatedEventArgs;
                if (_eventArgs.Arguments.Contains("url"))
                {

                    //lapage = typeof(actualite);
                }


            }
            Frame rootFrame = Window.Current.Content as Frame;

            // Ne répétez pas l'initialisation de l'application lorsque la fenêtre comporte déjà du contenu,
            // assurez-vous juste que la fenêtre est active
            if (rootFrame == null)
            {
                // Créez un Frame utilisable comme contexte de navigation et naviguez jusqu'à la première page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;
                rootFrame.Navigated += OnNavigated;
                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: chargez l'état de l'application précédemment suspendue
                }

                // Placez le frame dans la fenêtre active
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
                // Register a handler for BackRequested events and set the
                // visibility of the Back button
                SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;

            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                rootFrame.CanGoBack ?
                AppViewBackButtonVisibility.Visible :
                AppViewBackButtonVisibility.Collapsed;
            {
                // Quand la pile de navigation n'est pas restaurée, accédez à la première page,
                // puis configurez la nouvelle page en transmettant les informations requises en tant que
                // paramètre

                LaunchActivatedEventArgs _eventArgs = e as LaunchActivatedEventArgs;
                rootFrame.Navigate(lapage, e.Arguments);
            }
            // Vérifiez que la fenêtre actuelle est active
            Window.Current.Activate();
        }

        private void OnNavigated(object sender, NavigationEventArgs e)
        {
            // Each time a navigation event occurs, update the Back button's visibility
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                ((Frame)sender).CanGoBack ?
                AppViewBackButtonVisibility.Visible :
                AppViewBackButtonVisibility.Collapsed;
        }
        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>

        private void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame.CanGoBack)
            {
                e.Handled = true;
                rootFrame.GoBack();
            }
            else
            {

                e.Handled = true;
                App.Current.Exit();
            }
        }

        /// <summary>
        /// Appelé lorsque la navigation vers une page donnée échoue
        /// </summary>
        /// <param name="sender">Frame à l'origine de l'échec de navigation.</param>
        /// <param name="e">Détails relatifs à l'échec de navigation</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Appelé lorsque l'exécution de l'application est suspendue.  L'état de l'application est enregistré
        /// sans savoir si l'application pourra se fermer ou reprendre sans endommager
        /// le contenu de la mémoire.
        /// </summary>
        /// <param name="sender">Source de la requête de suspension.</param>
        /// <param name="e">Détails de la requête de suspension.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: enregistrez l'état de l'application et arrêtez toute activité en arrière-plan
            deferral.Complete();
        }



        protected override void OnActivated(IActivatedEventArgs e)
        {


#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif

            string data = null;

            Type lapage = typeof(MainPage);
            // Handle toast activation
            if (e is ToastNotificationActivatedEventArgs)
            {
                var toastActivationArgs = e as ToastNotificationActivatedEventArgs;

                // Parse the query string (using QueryString.NET)
                QueryString args = QueryString.Parse(toastActivationArgs.Argument);
                data = args["url"];

              //  lapage = typeof(actualite2);

            }
            Frame rootFrame = Window.Current.Content as Frame;

            // Ne répétez pas l'initialisation de l'application lorsque la fenêtre comporte déjà du contenu,
            // assurez-vous juste que la fenêtre est active
            if (rootFrame == null)
            {
                // Créez un Frame utilisable comme contexte de navigation et naviguez jusqu'à la première page
                rootFrame = new Frame();

                // TODO: modifier cette valeur à une taille de cache qui contient à votre application
                rootFrame.CacheSize = 1;



                // Placez le frame dans la fenêtre active
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {




                // Quand la pile de navigation n'est pas restaurée, accédez à la première page,
                // puis configurez la nouvelle page en transmettant les informations requises en tant que
                // paramètre

            }

            if (!rootFrame.Navigate(lapage, data))
            {
                throw new Exception("Failed to create initial page");
            }
            // Vérifiez que la fenêtre actuelle est active
            Window.Current.Activate();

        }

    }
}
