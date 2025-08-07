using DataBase.Data;

using Shared.Data;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Data.InventoryApi
{
    public partial class GetDayHttp
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IAccessDataBase _db;
        private readonly string _url;

        public GetDayHttp(IHttpClientFactory httpClientFactory, IAccessDataBase db)
        {
            _httpClientFactory = httpClientFactory;
            _db = db;
            _url = db.GetServerUrl();
        }



    }
}
