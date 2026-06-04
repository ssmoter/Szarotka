namespace Shared.Helper
{
    public static class AnimationsExtensions
    {
        public static async Task<bool> BounceOnPressAsync(this View view)
        {
            await view.ScaleTo(1.2, 100, Easing.BounceIn);

            return await view.ScaleTo(1.0, 100, Easing.BounceOut);
        }

    }
}
