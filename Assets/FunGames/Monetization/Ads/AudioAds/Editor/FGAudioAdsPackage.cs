using FunGames.Tools.Debugging.Editor;

namespace FunGames.Monetization.AudioAds
{
    public class FGAudioAdsPackage : FGPackageAbstract<FGAudioAdsPackage>
    {
        public override string JsonName => "";

        public override string ModulePath => FGPackageFolders.MONETIZATION + "/" + FGPackageFolders.ADS + "/" +
                                             FGPackageFolders.AUDIO_ADS;
    }
}