using BAL.Helpers;

namespace Backend.Helpers
{
    public static class RenderDelayHelper
    {
        public static async Task Delay()
        {
            await Task.Delay(MagicHelper.NeedDelayRefresh);
        }
    }
}
