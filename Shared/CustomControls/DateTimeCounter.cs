namespace Shared.CustomControls
{
    public partial class DateTimeCounter : Label, IDisposable
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
        private readonly Timer _timer;

        public DateTimeCounter()
        {
            var period = TimeSpan.FromSeconds(5);
            _timer = new Timer(TimerCallBack, null, TimeSpan.Zero, period);
        }

        private void TimerCallBack(object state)
        {
            if (DateTimeFrom == DateTime.MinValue)
            {
                return;
            }
            Dispatcher.Dispatch(() =>
            {
                var period = Date - DateTimeFrom;
                var _text = CalculateTime(period);
                this.Text = _text;
            });
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

        public void Dispose()
        {
            if (_timer is not null)
            {
                _timer.Dispose();
            }
        }
    }
}
