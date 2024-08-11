namespace DataBase.Data.File
{
    public static class FileHelper
    {
        public const string jsonTyp = ".json";
        public const string txtTyp = ".txt";
        public const string JsonFolder = "Json";
        public const string csvTyp = ".csv";
        public const string CsvFolder = "CSV";
        public const string DriversRoutes = "Trasy";

        public static IList<string> GetFilesPaths(string typ)
        {
            var path = Path.Combine(DataBase.Helper.Constants.GetPathFolder, typ);

            if (!System.IO.Directory.Exists(path))
            {
                return null;
            }

            var list = Directory.GetFiles(path);

            return list;
        }

        public static string FileIsExist(string path, string name, string typ)
        {
            int? number = 0;
            var privatePath = Path.Combine(path, name + typ);
            for (int i = 0; ; i++)
            {
                if (!System.IO.File.Exists(privatePath))
                {
                    break;
                }
                number++;
                privatePath = Path.Combine(path, name + "_" + number + typ);
            }

            return privatePath;
        }

        public static void CreateFolder(string path)
        {
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
        }


    }
}
