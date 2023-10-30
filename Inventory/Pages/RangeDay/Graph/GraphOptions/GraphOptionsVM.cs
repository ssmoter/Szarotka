using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Inventory.Pages.RangeDay.Graph.GraphOptions
{
    public partial class GraphOptionsVM : ObservableObject, IDisposable
    {
        [ObservableProperty]
        GraphOptionsM graphOptionsMs;

        public Func<object, Task> Close;
        public Task OnClose(object result = null)
        {
            return Close?.Invoke(result);
        }


        public GraphOptionsVM(string[] productNames)
        {
            GraphOptionsMs ??= new();
            GraphOptionsMs.ProductMs = new GraphOptionsProductM[productNames.Length];

            for (int i = 0; i < productNames.Length; i++)
            {
                GraphOptionsMs.ProductMs[i] = new GraphOptionsProductM() { Name = productNames[i] };
            }
        }

        [RelayCommand]
        async Task SaveAndReturn()
        {
            try
            {
                await OnClose(GraphOptionsMs);
            }
            finally { Dispose(); }
        }

        [RelayCommand]
        async Task CancelAndReturn()
        {
            try
            {
                await OnClose(null);
            }
            finally { Dispose(); }
        }

        public void Dispose()
        {
            GraphOptionsMs.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
