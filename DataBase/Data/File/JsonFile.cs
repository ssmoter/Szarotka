﻿using Newtonsoft.Json;

using System.Text;

namespace DataBase.Data.File
{
    public static class JsonFile
    {
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
                while ((numRead = await sourceStream.ReadAsync(buffer)) != 0)
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
        public static async Task<string> SaveFileJson(object model, string name, string folderName = FileHelper.JsonFolder)
        {
            try
            {
                var json = JsonConvert.SerializeObject(model);
                byte[] encodedtext = Encoding.Unicode.GetBytes(json);

                var path = Path.Combine(DataBase.Helper.Constants.GetPathFolder, folderName);

                FileHelper.CreateFolder(path);

                path = FileHelper.FileIsExist(path, name, FileHelper.jsonTyp);

                using var stream = new FileStream(path, FileMode.CreateNew, FileAccess.Write, FileShare.None, bufferSize: 4096, useAsync: true);
                await stream.WriteAsync(encodedtext);

                return path;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
