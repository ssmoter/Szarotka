using System;
using System.Collections.Generic;
using System.Text;

namespace DataBase.Helper
{
    public static class TaskExtensions
    {
        public static async void FireAndForget(this Task task)
        {
            try
            {
                await task;
            }
            catch
            {
                // Celowo puste - błędy są już obsłużone 
                // wewnątrz przekazanego Taska
            }
        }
    }
}
