using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HearthStone_HelperStats.Libs
{
    class JSONHelper
    {
        public static string JsonSerializer<T>(T t)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            MemoryStream ms = new MemoryStream();
            ser.WriteObject(ms, t);
            string jsonString = Encoding.UTF8.GetString(ms.ToArray());
            ms.Dispose();
            return jsonString;
        }

        public static T JsonDeserialize<T>(string JsonFile)
        {
            using (MemoryStream ms = new MemoryStream())
            using (FileStream file = new FileStream(JsonFile, FileMode.Open, FileAccess.Read))
            {
                byte[] bytes = new byte[file.Length];
                file.Read(bytes, 0, (int)file.Length);
                ms.Write(bytes, 0, (int)file.Length);
                ms.Position = 0;

                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
                T obj = (T)ser.ReadObject(ms);
                return obj;
            }
        }
    }
}
