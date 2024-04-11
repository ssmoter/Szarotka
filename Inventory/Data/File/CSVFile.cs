using CsvHelper;

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

                    model[i].Day.Id = csv.GetField<Guid>("Dzień_Id");
                    model[i].Day.DriverGuid = csv.GetField<Guid>("Dzień_Kierowca_Id");
                    model[i].Day.Description = csv.GetField<string>("Dzień_Opis");
                    model[i].Day.Created = csv.GetField<DateTime>("Dzień_Data");
                    model[i].Day.TotalPriceProductsDecimal = csv.GetField<decimal>("Dzień_Utarg_Produkty");
                    model[i].Day.TotalPriceCakeDecimal = csv.GetField<decimal>("Dzień_Utarg_Ciasto");
                    model[i].Day.TotalPriceDecimal = csv.GetField<decimal>("Dzień_Utarg_Suma");
                    model[i].Day.TotalPriceCorrectDecimal = csv.GetField<decimal>("Dzień_Utarg_Korekta");
                    model[i].Day.TotalPriceAfterCorrectDecimal = csv.GetField<decimal>("Dzień_Utarg_Po_Korekcie");
                    model[i].Day.TotalPriceMoneyDecimal = csv.GetField<decimal>("Dzień_Pieniądze");
                    model[i].Day.TotalPriceDifferenceDecimal = csv.GetField<decimal>("Dzień_Różnica");

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
                        model[i].Day.Products.Add(new());




                        model[i].Day.Products[j].Name.Name = csv.GetField<string>("Produkt_Nazwa");

                        model[i].Day.Products[j].Number = csv.GetField<int>("Produkt_Ilość");
                        model[i].Day.Products[j].NumberEdit = csv.GetField<int>("Produkt_Ilość_Edycja");
                        model[i].Day.Products[j].NumberReturn = csv.GetField<int>("Produkt_Ilość_zwrot");
                        model[i].Day.Products[j].PriceTotalDecimal = csv.GetField<decimal>("Produkt_Utarg");
                        model[i].Day.Products[j].PriceTotalCorrectDecimal = csv.GetField<decimal>("Produkt_Utarg_Edycja");
                        model[i].Day.Products[j].PriceTotalAfterCorrectDecimal = csv.GetField<decimal>("Produkt_Utarg_Po_Edycji");

                        model[i].Day.Products[j].Price.PriceDecimal = csv.GetField<decimal>("Produkt_Cena_Wartość");

                        model[i].Day.Products[j].Price.Id = csv.GetField<Guid>("Produkt_Cena_Id");
                        model[i].Day.Products[j].Price.ProductNameId = csv.GetField<Guid>("Produkt_Cena_Nazwa_Id");
                        model[i].Day.Products[j].Price.Created = csv.GetField<DateTime>("Produkt_Cena_Stworzono");

                        model[i].Day.Products[j].Id = csv.GetField<Guid>("Produkt_Id");
                        model[i].Day.Products[j].DayId = csv.GetField<Guid>("Produkt_Dzień_Id");
                        model[i].Day.Products[j].ProductNameId = csv.GetField<Guid>("Produkt_Nazwa_Id");
                        model[i].Day.Products[j].ProductNameId = csv.GetField<Guid>("Produkt_Wybrana_Cena_Id");
                        model[i].Day.Products[j].Description = csv.GetField<string>("Produkt_Opis");

                        model[i].Day.Products[j].Name.Id = csv.GetField<Guid>("Produkt_Nazwa_Id");
                        model[i].Day.Products[j].Name.Description = csv.GetField<string>("Produkt_Nazwa_Opis");
                        model[i].Day.Products[j].Name.Img = csv.GetField<string>("Produkt_Nazwa_Img");

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
                        model[i].Day.Cakes.Add(new());
                        model[i].Day.Cakes[j].PriceDecimal = csv.GetField<decimal>("Ciasto_Cena");
                        model[i].Day.Cakes[j].IsSell = csv.GetField<bool>("Ciasto_CzySprzedane");

                        model[i].Day.Cakes[j].Id = csv.GetField<Guid>("Ciasto_Id");
                        model[i].Day.Cakes[j].DayId = csv.GetField<Guid>("Ciasto_Dzień_Id");

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
                    csv.WriteField(model.Sum(x => x.Day.TotalPriceMoneyDecimal));
                    csv.WriteField(model.Sum(x => x.Day.TotalPriceAfterCorrectDecimal));
                    csv.WriteField(model.Sum(x => x.Day.TotalPriceDifferenceDecimal));

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
                        csv.WriteField(model[i].Day.Id);
                        csv.WriteField(model[i].Day.DriverGuid);
                        csv.WriteField(model[i].Day.Description);
                        csv.WriteField(model[i].Day.Created);
                        csv.WriteField(model[i].Day.TotalPriceProductsDecimal);
                        csv.WriteField(model[i].Day.TotalPriceCakeDecimal);
                        csv.WriteField(model[i].Day.TotalPriceDecimal);
                        csv.WriteField(model[i].Day.TotalPriceCorrectDecimal);
                        csv.WriteField(model[i].Day.TotalPriceAfterCorrectDecimal);
                        csv.WriteField(model[i].Day.TotalPriceMoneyDecimal);
                        csv.WriteField(model[i].Day.TotalPriceDifferenceDecimal);

                        csv.NextRecord();

                        CSVWriteHeader.Product(csv);
                        csv.NextRecord();
                        for (int j = 0; j < model[i].Day.Products.Count; j++)
                        {
                            csv.WriteField(model[i].Day.Products[j].Name.Name);

                            csv.WriteField(model[i].Day.Products[j].Number);
                            csv.WriteField(model[i].Day.Products[j].NumberEdit);
                            csv.WriteField(model[i].Day.Products[j].NumberReturn);
                            csv.WriteField(model[i].Day.Products[j].PriceTotalDecimal);
                            csv.WriteField(model[i].Day.Products[j].PriceTotalCorrectDecimal);
                            csv.WriteField(model[i].Day.Products[j].PriceTotalAfterCorrectDecimal);

                            csv.WriteField(model[i].Day.Products[j].Price.PriceDecimal);

                            csv.WriteField(model[i].Day.Products[j].Price.Id);
                            csv.WriteField(model[i].Day.Products[j].Price.ProductNameId);
                            csv.WriteField(model[i].Day.Products[j].Price.Created);

                            csv.WriteField(model[i].Day.Products[j].Id);
                            csv.WriteField(model[i].Day.Products[j].DayId);
                            csv.WriteField(model[i].Day.Products[j].ProductNameId);
                            csv.WriteField(model[i].Day.Products[j].ProductPriceId);
                            csv.WriteField(model[i].Day.Products[j].Description);

                            csv.WriteField(model[i].Day.Products[j].Name.Id);
                            csv.WriteField(model[i].Day.Products[j].Name.Description);
                            csv.WriteField(model[i].Day.Products[j].Name.Img);

                            csv.NextRecord();
                        }

                        CSVWriteHeader.Cake(csv);
                        csv.NextRecord();
                        for (int j = 0; j < model[i].Day.Cakes.Count; j++)
                        {

                            csv.WriteField(model[i].Day.Cakes[j].PriceDecimal);
                            csv.WriteField(model[i].Day.Cakes[j].IsSell);

                            csv.WriteField(model[i].Day.Cakes[j].Id);
                            csv.WriteField(model[i].Day.Cakes[j].DayId);

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
                date = $"{model.FirstOrDefault().Day.Created.ToShortDateString()}_{model.LastOrDefault().Day.Created.ToShortDateString()}";
            }
            else
            {
                date = model.FirstOrDefault().Day.Created.ToShortDateString();
            }


            return date;
        }
    }
}
