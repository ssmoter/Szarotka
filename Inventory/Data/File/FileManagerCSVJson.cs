using Newtonsoft.Json;

using System.Text;

namespace Inventory.Data.File
{
    public static class FileManagerCSVJson
    {
        const string jsonTyp = ".json";
        const string jsonFolder = "Json";

        public static Task<object> GetFileCSV(string path)
        {
            throw new NotImplementedException();
        }

        public static async Task<T> GetFileJson<T>(string path)
        {
            try
            {
                using var sourceStream = new FileStream(path,
                      FileMode.Open, FileAccess.Read, FileShare.Read,
                      bufferSize: 4096, useAsync: true);

                StringBuilder sb = new();

                byte[] buffer = new byte[0x1000];
                int numRead;
                while ((numRead = await sourceStream.ReadAsync(buffer, 0, buffer.Length)) != 0)
                {
                    string text = Encoding.Unicode.GetString(buffer, 0, numRead);
                    sb.Append(text);
                }

                var model = JsonConvert.DeserializeObject<T>(sb.ToString());
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static async Task<bool> SaveFileCSV(object model, string name)
        {
            throw new NotImplementedException();
        }

        public static async Task<bool> SaveFileJson(object model, string name)
        {
            try
            {
                var json = JsonConvert.SerializeObject(model);
                byte[] encodedtext = Encoding.Unicode.GetBytes(json);

                var path = Path.Combine(DataBase.Helper.Constants.GetPathFolder, jsonFolder);

                CreateFolder(path);

                path = FileIsExist(path, name);

                using var stream = new FileStream(path, FileMode.CreateNew, FileAccess.Write, FileShare.None, bufferSize: 4096, useAsync: true);
                await stream.WriteAsync(encodedtext, 0, encodedtext.Length);

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }


        static string FileIsExist(string path, string name)
        {
            int? number = 0;
            var privatePath = Path.Combine(path, name + jsonTyp);
            for (int i = 0; ; i++)
            {
                if (!System.IO.File.Exists(privatePath))
                {
                    break;
                }
                number++;
                privatePath = Path.Combine(path, name + "_" + number + jsonTyp);
            }

            return privatePath;
        }

        static void CreateFolder(string path)
        {
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
        }

    }
}
