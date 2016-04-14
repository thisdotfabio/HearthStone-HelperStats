using System;
using System.Net;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;
using Windows.Storage;
using Windows.Web.Http;

namespace HearthStone_HelperStats.Libs
{
    static class JSONCard
    {
        public static JSONCardDownloadStatus UpdateCardList()
        {
            JSONCardDownloadStatus returnValue = JSONCardDownloadStatus.NotDownloaded;

            Task.WaitAll(
                Task.Run(async () =>
           {
               try
               {
                   if (NetworkInformation.GetInternetConnectionProfile() != null)
                   {
                       StorageFolder folder = ApplicationData.Current.LocalFolder;
                       String lastUpdate, webUpdate;

                       WebRequest request = HttpWebRequest.Create("https://api.hearthstonejson.com/v1/latest/esMX/cards.collectible.json");
                       request.Method = "HEAD";
                       using (WebResponse response = await request.GetResponseAsync())
                       {
                           webUpdate = response.Headers[HttpRequestHeader.LastModified];
                       }

                       lastUpdate = await StorageFileStream.LoadAsync<string>("LastUpdate");

                       if (lastUpdate == null || lastUpdate != webUpdate)
                       {
                           HttpClient client = new HttpClient();
                           HttpResponseMessage message = await client.GetAsync(new Uri("https://api.hearthstonejson.com/v1/latest/esMX/cards.collectible.json"));

                           StorageFile cards = await folder.CreateFileAsync("cards.collectible.json", CreationCollisionOption.ReplaceExisting);
                           Windows.Storage.Streams.IBuffer b = await message.Content.ReadAsBufferAsync();

                           await FileIO.WriteBufferAsync(cards, b);

                           await StorageFileStream.SaveAsync("LastUpdate", webUpdate);

                           returnValue = JSONCardDownloadStatus.Downloaded;
                       }
                       else
                           returnValue = JSONCardDownloadStatus.AlreadyUpdated;
                   }
                   else
                       returnValue = JSONCardDownloadStatus.NotDownloaded;
               }
               catch (Exception e)
               {
                   returnValue = JSONCardDownloadStatus.NotDownloaded;
               }
           })
           );

            return returnValue;
        }
    }
}
