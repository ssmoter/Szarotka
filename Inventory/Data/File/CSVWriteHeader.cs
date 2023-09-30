using CsvHelper;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Data.File
{
    public static class CSVWriteHeader
    {
        public static void Driver(CsvWriter csv)
        {
            csv.WriteField("Kierowca_Id");
            csv.WriteField("Kierowca_Nazwa");
            csv.WriteField("Kierowca_Opis");
        }
        public static void Day(CsvWriter csv)
        {
            csv.WriteField("Dzień_Id");
            csv.WriteField("Dzień_Kierowca_Id");
            csv.WriteField("Dzień_Opis");
            csv.WriteField("Dzień_Data");
            csv.WriteField("Dzień_Utarg_Produkty");
            csv.WriteField("Dzień_Utarg_Ciasto");
            csv.WriteField("Dzień_Utarg_Suma");
            csv.WriteField("Dzień_Utarg_Korekta");
            csv.WriteField("Dzień_Utarg_Po_Korekcie");
            csv.WriteField("Dzień_Pieniądze");
            csv.WriteField("Dzień_Różnica");
        }

        public static void Product(CsvWriter csv)
        {

            csv.WriteField("Produkt_Nazwa");

            csv.WriteField("Produkt_Ilość");
            csv.WriteField("Produkt_Ilość_Edycja");
            csv.WriteField("Produkt_Ilość_zwrot");
            csv.WriteField("Produkt_Utarg");
            csv.WriteField("Produkt_Utarg_Edycja");
            csv.WriteField("Produkt_Utarg_Po_Edycji");

            csv.WriteField("Produkt_Cena_Wartość");

            csv.WriteField("Produkt_Cena_Id");
            csv.WriteField("Produkt_Cena_Nazwa_Id");
            csv.WriteField("Produkt_Cena_Stworzono");

            csv.WriteField("Produkt_Id");
            csv.WriteField("Produkt_Dzień_Id");
            csv.WriteField("Produkt_Nazwa_Id");
            csv.WriteField("Produkt_Wybrana_Cena_Id");
            csv.WriteField("Produkt_Opis");

            csv.WriteField("Produkt_Nazwa_Id");
            csv.WriteField("Produkt_Nazwa_Opis");
            csv.WriteField("Produkt_Nazwa_Img");
        }
        public static void Cake(CsvWriter csv)
        {
            csv.WriteField("Ciasto_Cena");
            csv.WriteField("Ciasto_CzySprzedane");

            csv.WriteField("Ciasto_Id");
            csv.WriteField("Ciasto_Dzień_Id");
        }
    }
}
