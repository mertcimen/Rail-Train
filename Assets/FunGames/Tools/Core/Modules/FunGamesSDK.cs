namespace FunGames.Tools.Core.Modules
{
    public class FunGamesSDK
    {
        public static FGModuleCallbacks Callbacks => FGMain.Instance.Callbacks;
        
        public static bool IsInitialized => FGMain.Instance.IsInitialized();

        public static void RemoveAds() => FGMain.Instance.RemoveAds();
        public static bool IsNoAds => FGMain.Instance.IsNoAd();
        public static bool IsFirstConnection => FGMain.Instance.IsFirstConnection();
        public static int DaysSinceFirstCo => FGMain.Instance.DaysSinceFirstConnection();
        public static int DaysSinceLastCo => FGMain.Instance.DaysSinceLastConnection();
    }
}