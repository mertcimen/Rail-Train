namespace FunGames.Monetization.Crosspromo
{
    public class FGCrosspromo
    {
        public static FGCrosspromoCallbacks Callbacks => FGCrosspromoManager.Instance.Callbacks;

        public static void Play() => FGCrosspromoManager.Instance.Play();
        public static void Close() => FGCrosspromoManager.Instance.Close();

        // public static void Completed(bool success) => FGCrosspromoManager.Instance.Completed(success);
    }
}