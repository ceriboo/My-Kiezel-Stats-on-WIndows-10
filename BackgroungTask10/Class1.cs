using Microsoft.Toolkit.Uwp.Notifications;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Storage;
using Windows.UI.Notifications;

namespace BackgroungTask10
{
    public sealed class BGTUWP : IBackgroundTask
    {
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            Debug.WriteLine("ça marche");
            BackgroundTaskDeferral deferral = taskInstance.GetDeferral();


            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36");
            HttpResponseMessage laresponse = new HttpResponseMessage();
            laresponse = await client.GetAsync(("http://www.airparif.asso.fr/appli/api/1.1/indice"));

            if (laresponse.IsSuccessStatusCode)
            {
                var toto = await laresponse.Content.ReadAsStringAsync();
                toto = "{\"indicedesjours\":" + toto.ToString() + "}";
                Debug.WriteLine(toto);


                var resultat = JsonConvert.DeserializeObject<indiceairparif>(toto);


                //   updatetile(resultat.indicedesjours[1].url_carte, resultat.indicedesjours[1].indice.ToString());
                deferral.Complete();
            }
        }

        class Indicedesjour
        {
            public string date { get; set; }
            public string url_carte { get; set; }
            public int? indice { get; set; }
        }

        class indiceairparif
        {
            public List<Indicedesjour> indicedesjours { get; set; }
        }

        public static async void updatetile(string image, string text)

        {
            Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;

            StorageFile file = await storageFolder.CreateFileAsync("airparif.jpg", Windows.Storage.CreationCollisionOption.ReplaceExisting);

            HttpClient client = new HttpClient(); // Create HttpClient
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36");

            byte[] buffer = await client.GetByteArrayAsync(image); // Download file
            using (Stream stream = await file.OpenStreamForWriteAsync())
                stream.Write(buffer, 0, buffer.Length


                    ); // Save
            await file.CopyAsync(ApplicationData.Current.LocalFolder, "airparif" + ".jpg", NameCollisionOption.ReplaceExisting);


            var tileContent = new TileContent()
            {
                Visual = new TileVisual()
                {
                    TileSmall = new TileBinding()
                    {
                        Content = new TileBindingContentAdaptive()
                        {
                            BackgroundImage = new TileBackgroundImage()
                            {
                                Source = "Assets/Apps/Maps/TileLogoSmall.png"
                            },
                            PeekImage = new TilePeekImage()
                            {
                                Source = "Assets/Apps/Maps/map.jpg",
                                HintOverlay = 20
                            }
                        }
                    },
                    TileMedium = new TileBinding()
                    {
                        Content = new TileBindingContentAdaptive()
                        {
                            BackgroundImage = new TileBackgroundImage()
                            {
                                Source = "Assets/Apps/Maps/TileLogoMedium.png"
                            },
                            PeekImage = new TilePeekImage()
                            {
                                Source = "Assets/Apps/Maps/map.jpg",
                                HintOverlay = 20
                            }
                        }
                    },
                    TileWide = new TileBinding()
                    {
                        Content = new TileBindingContentAdaptive()
                        {
                            BackgroundImage = new TileBackgroundImage()
                            {
                                Source = "Assets/Apps/Maps/TileLogoWide.png"
                            },
                            PeekImage = new TilePeekImage()
                            {
                                Source = "Assets/Apps/Maps/map.jpg",
                                HintOverlay = 20
                            }
                        }
                    },
                    TileLarge = new TileBinding()
                    {
                        Content = new TileBindingContentAdaptive()
                        {
                            BackgroundImage = new TileBackgroundImage()
                            {
                                Source = "Assets/Apps/Maps/TileLogoLarge.png"
                            },
                            PeekImage = new TilePeekImage()
                            {
                                Source = "Assets/Apps/Maps/map.jpg",
                                HintOverlay = 20
                            }
                        }
                    }
                }
            };

            // Create the tile notification
            var tileNotif = new TileNotification(tileContent.GetXml());

            // And send the notification to the primary tile
            TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotif);

        }
    }
}
