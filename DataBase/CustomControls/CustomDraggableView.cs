namespace DataBase.CustomControls
{
    public partial class CustomDraggableBorder : Border, IDisposable
    {
        private double initialWidth;
        private double initialHeight;
        private double startX;
        private double startY;

        private double startXMax;
        private double startYMax;

        private readonly PanGestureRecognizer panGesture;
        public CustomDraggableBorder()
        {
            panGesture = new PanGestureRecognizer();
            panGesture.PanUpdated += OnPanUpdated;
            this.GestureRecognizers.Add(panGesture);
        }

        public void Dispose()
        {
            panGesture.PanUpdated -= OnPanUpdated;
        }

        void OnPanUpdated(object sender, PanUpdatedEventArgs e)
        {
            var parent = (this.Parent as View);
            startXMax = parent.Width;
            startYMax = parent.Height;

            switch (e.StatusType)
            {
                case GestureStatus.Started:
                    initialWidth = Width;
                    initialHeight = Height;
                    startX = e.TotalX;
                    startY = e.TotalY;
                    break;
                case GestureStatus.Running:
                    double newWidth = initialWidth + (-e.TotalX - startX);
                    double newHeight = initialHeight + (e.TotalY - startY);

                    // Optional: Set a minimum size for the view
                    var x = Math.Max(newWidth, 50);
                    if (startXMax > x)
                    {
                        WidthRequest = x;
                    }
                    var y = Math.Max(newHeight, 50);
                    if (startYMax > y)
                    {
                        HeightRequest = y;
                    }
                    break;
                case GestureStatus.Completed:
                    // Optionally do something when resizing is complete
                    break;
            }
        }
    }

}
