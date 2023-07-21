using FunGames.Tools;
using FunGames.Tools.Core.Modules;

namespace FunGames.Monetization.Crosspromo
{
    public abstract class FGCrosspromoAbstract<T> : FGModuleChild<T, FGCrosspromoCallbacks> where T : FGModuleChild<T, FGCrosspromoCallbacks>
    {
        protected override IFGModuleParent Parent => FGCrosspromoManager.Instance;
        
        protected override void InitializeCallbacks()
        {
            FGCrosspromoManager.Instance.Callbacks.Initialization += Initialize;
            FGCrosspromoManager.Instance.Callbacks.OnPlay += Play;
            FGCrosspromoManager.Instance.Callbacks.OnClose += Close;
            FGCrosspromoManager.Instance.Callbacks.OnCompleted += Completed;
            FGCrosspromoManager.Instance.Callbacks.OnCompleted += LogVideoCompletion;
        }

        protected abstract void Play();
        
        protected abstract void Close();
        
        protected abstract void Completed(bool success);

        private void LogVideoCompletion(bool success)
        {
            if (success)
            {
                Log("Video completed successfully !");
            }
            else
            {
                LogWarning("Video not completed!");
            }
        }

        protected override void ClearInitialization()
        {
            FGCrosspromoManager.Instance.Callbacks.Initialization -= Initialize;
            FGCrosspromoManager.Instance.Callbacks.OnPlay -= Play;
            FGCrosspromoManager.Instance.Callbacks.OnClose -= Close;
            FGCrosspromoManager.Instance.Callbacks.OnCompleted -= Completed;
            FGCrosspromoManager.Instance.Callbacks.OnCompleted -= LogVideoCompletion;
        }
        
        public override bool MustBeInitialized()
        {
            return base.MustBeInitialized() && !FunGamesSDK.IsNoAds;
        }
    }
}