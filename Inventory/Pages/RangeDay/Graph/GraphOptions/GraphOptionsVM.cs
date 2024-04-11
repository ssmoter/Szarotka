using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Inventory.Pages.RangeDay.Graph.GraphOptions
{
    public partial class GraphOptionsVM : ObservableObject, IDisposable
    {
        [ObservableProperty]
        GraphOptionsM graphOptionsMs;

        public Func<object, CancellationToken, Task> Close;
        public Task OnClose(object result = null, CancellationToken token = default)
        {
            return Close?.Invoke(result, token);
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
