namespace Shared.CustomControls
{
    public partial class DateTimeCounter : Label
    {
        public static readonly BindableProperty DateTimeFromProperty
    = BindableProperty.Create(nameof(DateTimeFrom),
        typeof(DateTime),
        typeof(DateTimeCounter),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanged: (bindable, oldValue, newValue) =>
        {
        });

        public DateTime DateTimeFrom
        {
            get => (DateTime)GetValue(DateTimeFromProperty);
            set => SetValue(DateTimeFromProperty, value);
        }


        private static DateTime Date => DateTime.Now;
        private PeriodicTimer _timer;
        private CancellationTokenSource _cts;

        public DateTimeCounter()
        {
        }

        protected override void OnHandlerChanged()
        {
            base.OnHandlerChanged();

            // Upewnij się, że nie uruchamiasz wielu timerów
            _cts?.Cancel();
            _cts = new CancellationTokenSource();

            _timer = new PeriodicTimer(TimeSpan.FromSeconds(1));
            _ = StartTimerAsync(_cts.Token);
        }

        private async Task StartTimerAsync(CancellationToken token)
        {
            try
            {
                {
                    while (await _timer.WaitForNextTickAsync(token))
                    {
                        if (DateTimeFrom == DateTime.MinValue)
                        {
                            continue;
                        }

                        Dispatcher.Dispatch(() =>
                        {
                            var period = Date - DateTimeFrom;
                            var _text = CalculateTime(period);
                            this.Text = _text;
                        });
                    }
                }
            }
            catch (OperationCanceledException)
            {
            }
        }

        private string CalculateTime(TimeSpan time)
        {
            if (time.TotalDays > 4)
            {
                if (DateTimeFrom == DateTime.MinValue)
                {
                    return "";
                }
                return DateTimeFrom.ToShortDateString();
            }
            if (time.TotalDays > 1)
            {
                return $"{(int)time.TotalDays} dni temu";
            }
            if (time.TotalHours > 1)
            {
                return $"{(int)time.TotalHours} godzin temu";
            }
            if (time.TotalMinutes > 1)
            {
                return $"{(int)time.TotalMinutes} minut temu";
            }

            return $"{(int)time.TotalSeconds} sekund temu";
        }
    }
}
