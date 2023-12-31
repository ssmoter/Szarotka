﻿using Inventory.Model.MVVM;

using System.Collections.ObjectModel;

namespace Inventory.Service
{
    public interface ISelectDayService
    {
        Task<ObservableCollection<CakeM>> GetCakeTable(DayM dayM);
        Task<DayM> GetDay();
        Task<DayM> GetDay(Guid id);
       // Task<DayM> GetDay(string createdDate);
        Task<DayM> GetDay(DateTime createdDate);
        //Task<DayM> GetDayProcedure(string createdDate);
        Task<DayM> GetDayProcedure(DateTime createdDate);
        Task<DayM> GetDayProcedure(Guid id);
        Task<ObservableCollection<ProductM>> GetProductTable(DayM dayM);
    }
}