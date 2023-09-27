using CsvHelper;

using Inventory.Pages.RangeDay;

using Newtonsoft.Json;

using System.Collections.ObjectModel;
using System.Text;

namespace Inventory.Data.File
{
    public static class FileManagerCSVJson
    {
        public const string jsonTyp = ".json";
        public const string JsonFolder = "Json";
        public const string csvTyp = ".csv";
        public const string CsvFolder = "CSV";

        public static ObservableCollection<RangeDayM> GetFileCSV(string path)
        {
            try
            {
                var model = new ObservableCollection<RangeDayM>();

                using var reader = new StreamReader(path);
                using var csv = new CsvReader(reader, DataBase.Helper.Constants.CultureInfo);
                for (int i = 0; ; i++)
                {

                    if (!csv.Read())
                    {
                        break;
                    }

                    if (i == 0)
                    {
                        csv.ReadHeader();
                        csv.Read();
                    }

                    model.Add(new RangeDayM());
                    model[i].Driver.Id = csv.GetField<Guid>("Kierowca_Id");
                    model[i].Driver.Name = csv.GetField<string>("Kierowca_Nazwa");
                    model[i].Driver.Description = csv.GetField<string>("Kierowca_Opis");
                    model[i].Driver.Guid = csv.GetField<Guid>("Kierowca_Guid");

                    csv.Read();
                    csv.ReadHeader();
                    csv.Read();

                    model[i].DayM.Id = csv.GetField<Guid>("Dzień_Id");
                    model[i].DayM.DriverGuid = csv.GetField<Guid>("Dzień_Kierowca_Id");
                    model[i].DayM.Description = csv.GetField<string>("Dzień_Opis");
                    model[i].DayM.Created = csv.GetField<DateTime>("Dzień_Data");
                    model[i].DayM.TotalPriceProduct = csv.GetField<decimal>("Dzień_Utarg_Produkty");
                    model[i].DayM.TotalPriceCake = csv.GetField<decimal>("Dzień_Utarg_Ciasto");
                    model[i].DayM.TotalPrice = csv.GetField<decimal>("Dzień_Utarg_Suma");
                    model[i].DayM.TotalPriceCorrect = csv.GetField<decimal>("Dzień_Utarg_Korekta");
                    model[i].DayM.TotalPriceAfterCorrect = csv.GetField<decimal>("Dzień_Utarg_Po_Korekcie");
                    model[i].DayM.TotalPriceMoney = csv.GetField<decimal>("Dzień_Pieniądze");
                    model[i].DayM.TotalPriceDifference = csv.GetField<decimal>("Dzień_Różnica");

                    csv.Read();
                    csv.ReadHeader();
                    if (!csv.Read())
                    {
                        break;
                    }

                    for (int j = 0; ; j++)
                    {
                        if ("Ciasto_Id" == csv.GetField(0))
                        {
                            break;
                        }
                        model[i].DayM.Products.Add(new());
                        model[i].DayM.Products[j].Id = csv.GetField<Guid>("Produkt_Id");
                        model[i].DayM.Products[j].DayId = csv.GetField<Guid>("Produkt_Dzień_Id");
                        model[i].DayM.Products[j].ProductNameId = csv.GetField<Guid>("Produkt_Nazwa_Id");
                        model[i].DayM.Products[j].Description = csv.GetField<string>("Produkt_Opis");

                        model[i].DayM.Products[j].Name.Id = csv.GetField<Guid>("Produkt_Nazwa_Id");
                        model[i].DayM.Products[j].Name.Description = csv.GetField<string>("Produkt_Nazwa_Opis");
                        model[i].DayM.Products[j].Name.Img = csv.GetField<string>("Produkt_Nazwa_Img");
                        model[i].DayM.Products[j].Name.Name = csv.GetField<string>("Produkt_Nazwa");

                        model[i].DayM.Products[j].Number = csv.GetField<int>("Produkt_Ilość");
                        model[i].DayM.Products[j].NumberEdit = csv.GetField<int>("Produkt_Ilość_Edycja");
                        model[i].DayM.Products[j].NumberReturn = csv.GetField<int>("Produkt_Ilość_zwrot");
                        model[i].DayM.Products[j].PriceTotal = csv.GetField<decimal>("Produkt_Utarg");
                        model[i].DayM.Products[j].PriceTotalCorrect = csv.GetField<decimal>("Produkt_Utarg_Edycja");
                        model[i].DayM.Products[j].PriceTotalAfterCorrect = csv.GetField<decimal>("Produkt_Utarg_Po_Edycji");

                        model[i].DayM.Products[j].Price.Price = csv.GetField<decimal>("Produkt_Cena_Wartość");
                        model[i].DayM.Products[j].Price.Id = csv.GetField<Guid>("Produkt_Cena_Id");
                        model[i].DayM.Products[j].Price.ProductNameId = csv.GetField<Guid>("Produkt_Cena_Nazwa_Id");
                        model[i].DayM.Products[j].Price.Created = csv.GetField<DateTime>("Produkt_Cena_Stworzono");
                        csv.Read();
                    }
                    csv.ReadHeader();
                    if (!csv.Read())
                    {
                        break;
                    }
                    for (int j = 0; ; j++)
                    {
                        if (csv.Parser is null)
                        {
                            break;
                        }

                        if ("Kierowca_Id" == csv.GetField(0))
                        {
                            csv.ReadHeader();
                            break;
                        }
                        model[i].DayM.Cakes.Add(new());
                        model[i].DayM.Cakes[j].Id = csv.GetField<Guid>("Ciasto_Id");
                        model[i].DayM.Cakes[j].DayId = csv.GetField<Guid>("Ciasto_Dzień_Id");
                        model[i].DayM.Cakes[j].Price = csv.GetField<decimal>("Ciasto_Cena");
                        model[i].DayM.Cakes[j].IsSell = csv.GetField<bool>("Ciasto_CzySprzedane");

                        csv.Read();

                    }
                }


                return model;
            }
            catch (Exception)
            {
                throw;
            }
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

        public static string SaveFileCSV(IList<Pages.RangeDay.RangeDayM> model, string name)
        {
            try
            {
                var path = Path.Combine(DataBase.Helper.Constants.GetPathFolder, CsvFolder);

                CreateFolder(path);

                path = FileIsExist(path, name, csvTyp);

                using (var writer = new StreamWriter(path))
                {
                    using (var csv = new CsvWriter(writer, DataBase.Helper.Constants.CultureInfo))
                    {
                        for (int i = 0; i < model.Count; i++)
                        {
                            csv.NextRecord();

                            CSVWriteHeader.Driver(csv);
                            csv.NextRecord();
                            csv.WriteField(model[i].Driver.Id);
                            csv.WriteField(model[i].Driver.Name);
                            csv.WriteField(model[i].Driver.Description);
                            csv.WriteField(model[i].Driver.Guid);
                            csv.NextRecord();

                            CSVWriteHeader.Day(csv);
                            csv.NextRecord();
                            csv.WriteField(model[i].DayM.Id);
                            csv.WriteField(model[i].DayM.DriverGuid);
                            csv.WriteField(model[i].DayM.Description);
                            csv.WriteField(model[i].DayM.Created);
                            csv.WriteField(model[i].DayM.TotalPriceProduct);
                            csv.WriteField(model[i].DayM.TotalPriceCake);
                            csv.WriteField(model[i].DayM.TotalPrice);
                            csv.WriteField(model[i].DayM.TotalPriceCorrect);
                            csv.WriteField(model[i].DayM.TotalPriceAfterCorrect);
                            csv.WriteField(model[i].DayM.TotalPriceMoney);
                            csv.WriteField(model[i].DayM.TotalPriceDifference);

                            csv.NextRecord();

                            CSVWriteHeader.Product(csv);
                            csv.NextRecord();
                            for (int j = 0; j < model[i].DayM.Products.Count; j++)
                            {
                                csv.WriteField(model[i].DayM.Products[j].Id);
                                csv.WriteField(model[i].DayM.Products[j].DayId);
                                csv.WriteField(model[i].DayM.Products[j].ProductNameId);
                                csv.WriteField(model[i].DayM.Products[j].Description);

                                csv.WriteField(model[i].DayM.Products[j].Name.Id);
                                csv.WriteField(model[i].DayM.Products[j].Name.Description);
                                csv.WriteField(model[i].DayM.Products[j].Name.Img);
                                csv.WriteField(model[i].DayM.Products[j].Name.Name);

                                csv.WriteField(model[i].DayM.Products[j].Number);
                                csv.WriteField(model[i].DayM.Products[j].NumberEdit);
                                csv.WriteField(model[i].DayM.Products[j].NumberReturn);
                                csv.WriteField(model[i].DayM.Products[j].PriceTotal);
                                csv.WriteField(model[i].DayM.Products[j].PriceTotalCorrect);
                                csv.WriteField(model[i].DayM.Products[j].PriceTotalAfterCorrect);


                                csv.WriteField(model[i].DayM.Products[j].Price.Price);
                                csv.WriteField(model[i].DayM.Products[j].Price.Id);
                                csv.WriteField(model[i].DayM.Products[j].Price.ProductNameId);
                                csv.WriteField(model[i].DayM.Products[j].Price.Created);

                                csv.NextRecord();
                            }

                            CSVWriteHeader.Cake(csv);
                            csv.NextRecord();
                            for (int j = 0; j < model[i].DayM.Cakes.Count; j++)
                            {
                                csv.WriteField(model[i].DayM.Cakes[j].Id);
                                csv.WriteField(model[i].DayM.Cakes[j].DayId);
                                csv.WriteField(model[i].DayM.Cakes[j].Price);
                                csv.WriteField(model[i].DayM.Cakes[j].IsSell);
                                csv.NextRecord();
                            }



                        }
                    }
                }
                return path;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static async Task<string> SaveFileJson(object model, string name)
        {
            try
            {
                var json = JsonConvert.SerializeObject(model);
                byte[] encodedtext = Encoding.Unicode.GetBytes(json);

                var path = Path.Combine(DataBase.Helper.Constants.GetPathFolder, JsonFolder);

                CreateFolder(path);

                path = FileIsExist(path, name, jsonTyp);

                using var stream = new FileStream(path, FileMode.CreateNew, FileAccess.Write, FileShare.None, bufferSize: 4096, useAsync: true);
                await stream.WriteAsync(encodedtext, 0, encodedtext.Length);

                return path;
            }
            catch (Exception)
            {
                throw;
            }
        }

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


        static string FileIsExist(string path, string name, string typ)
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

        static void CreateFolder(string path)
        {
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
        }

    }
}
