namespace FunGames.Mediation
{
    public interface IFGMediationAd
    {
        public void InitializeCallbacks();

        public void Load();

        public bool IsReady();  
    }
}