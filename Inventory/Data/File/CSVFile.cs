﻿using CsvHelper;

using DataBase.Data.File;

using Inventory.Pages.RangeDay;

using System.Text;

namespace Inventory.Data.File
{
    public static class CSVFile
    {
        public static RangeDayM[] GetFileCSV(string path)
        {
            try
            {
                var model = new List<RangeDayM>();

                using var reader = new StreamReader(path);
                using var csv = new CsvReader(reader, DataBase.Helper.Constants.CultureInfo);
                csv.Read();
                csv.Read();

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
                        if ("Ciasto_Cena" == csv.GetField(0))
                        {
                            break;
                        }
                        model[i].DayM.Products.Add(new());




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

                        model[i].DayM.Products[j].Id = csv.GetField<Guid>("Produkt_Id");
                        model[i].DayM.Products[j].DayId = csv.GetField<Guid>("Produkt_Dzień_Id");
                        model[i].DayM.Products[j].ProductNameId = csv.GetField<Guid>("Produkt_Nazwa_Id");
                        model[i].DayM.Products[j].ProductNameId = csv.GetField<Guid>("Produkt_Wybrana_Cena_Id");
                        model[i].DayM.Products[j].Description = csv.GetField<string>("Produkt_Opis");

                        model[i].DayM.Products[j].Name.Id = csv.GetField<Guid>("Produkt_Nazwa_Id");
                        model[i].DayM.Products[j].Name.Description = csv.GetField<string>("Produkt_Nazwa_Opis");
                        model[i].DayM.Products[j].Name.Img = csv.GetField<string>("Produkt_Nazwa_Img");

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
                        model[i].DayM.Cakes[j].Price = csv.GetField<decimal>("Ciasto_Cena");
                        model[i].DayM.Cakes[j].IsSell = csv.GetField<bool>("Ciasto_CzySprzedane");

                        model[i].DayM.Cakes[j].Id = csv.GetField<Guid>("Ciasto_Id");
                        model[i].DayM.Cakes[j].DayId = csv.GetField<Guid>("Ciasto_Dzień_Id");

                        csv.Read();

                    }
                }


                return model.ToArray();
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
                var path = Path.Combine(DataBase.Helper.Constants.GetPathFolder, FileHelper.CsvFolder);

                FileHelper.CreateFolder(path);

                path = FileHelper.FileIsExist(path, name, FileHelper.csvTyp);

                using (var writer = new StreamWriter(path, false, Encoding.UTF8))
                {
                    using var csv = new CsvWriter(writer, DataBase.Helper.Constants.CultureInfo);


                    csv.NextRecord();
                    CSVWriteHeader.WriteSum(csv);
                    csv.NextRecord();
                    csv.WriteField(SetRangeDay(model));
                    csv.WriteField(model.Sum(x => x.DayM.TotalPriceMoney));
                    csv.WriteField(model.Sum(x => x.DayM.TotalPriceAfterCorrect));
                    csv.WriteField(model.Sum(x => x.DayM.TotalPriceDifference));

                    csv.NextRecord();

                    for (int i = 0; i < model.Count; i++)
                    {
                        csv.NextRecord(); ;

                        CSVWriteHeader.Driver(csv);
                        csv.NextRecord();
                        csv.WriteField(model[i].Driver.Id);
                        csv.WriteField(model[i].Driver.Name);
                        csv.WriteField(model[i].Driver.Description);
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

                            csv.WriteField(model[i].DayM.Products[j].Id);
                            csv.WriteField(model[i].DayM.Products[j].DayId);
                            csv.WriteField(model[i].DayM.Products[j].ProductNameId);
                            csv.WriteField(model[i].DayM.Products[j].ProductPriceId);
                            csv.WriteField(model[i].DayM.Products[j].Description);

                            csv.WriteField(model[i].DayM.Products[j].Name.Id);
                            csv.WriteField(model[i].DayM.Products[j].Name.Description);
                            csv.WriteField(model[i].DayM.Products[j].Name.Img);

                            csv.NextRecord();
                        }

                        CSVWriteHeader.Cake(csv);
                        csv.NextRecord();
                        for (int j = 0; j < model[i].DayM.Cakes.Count; j++)
                        {

                            csv.WriteField(model[i].DayM.Cakes[j].Price);
                            csv.WriteField(model[i].DayM.Cakes[j].IsSell);

                            csv.WriteField(model[i].DayM.Cakes[j].Id);
                            csv.WriteField(model[i].DayM.Cakes[j].DayId);

                            csv.NextRecord();
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

        static string SetRangeDay(IList<Pages.RangeDay.RangeDayM> model)
        {
            string date;
            if (model.Count > 1)
            {
                date = $"{model.FirstOrDefault().DayM.Created.ToShortDateString()}_{model.LastOrDefault().DayM.Created.ToShortDateString()}";
            }
            else
            {
                date = model.FirstOrDefault().DayM.Created.ToShortDateString();
            }


            return date;
        }
    }
}
