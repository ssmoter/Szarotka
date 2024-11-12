namespace DriversRoutes.Model
{
    public struct PinSize(int scaleX, int scaleY, int width, int height)
    {
        public int ScaleX = scaleX;
        public int ScaleY = scaleY;
        public int Width = width;
        public int Height = height;
    }
}
