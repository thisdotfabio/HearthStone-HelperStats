using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Windows.Storage;
using Windows.Storage.Streams;

namespace HearthStone_HelperStats.Libs
{
    class StorageFileStream
    {

        public static async Task SaveAsync<T>(string Key, T Obj)
        {
            try
            {
                if (Obj != null)
                {
                    StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync(Key, CreationCollisionOption.ReplaceExisting);
                    IRandomAccessStream writeStream = await file.OpenAsync(FileAccessMode.ReadWrite);
                    using (Stream outStream = Task.Run(() => writeStream.AsStreamForWrite()).Result)
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(T));
                        serializer.Serialize(outStream, Obj);
                        await outStream.FlushAsync();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static async Task DeleteAsync(string Key)
        {
            try
            {
                StorageFile file = await ApplicationData.Current.LocalFolder.GetFileAsync(Key);
                if (file != null)
                {
                    await file.DeleteAsync(StorageDeleteOption.PermanentDelete);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static async Task<T> LoadAsync<T>(string Key)
        {
            try
            {
                StorageFile file = await ApplicationData.Current.LocalFolder.GetFileAsync(Key);
                IRandomAccessStream readStream = await file.OpenAsync(FileAccessMode.Read);
                using (Stream inStream = Task.Run(() => readStream.AsStreamForRead()).Result)
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(T));
                    return (T)serializer.Deserialize(inStream);
                }
            }
            catch (FileNotFoundException)
            {
                return default(T);
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
