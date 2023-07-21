using System;

[Serializable]
public class FGAdUnit
{
    public string unitId;
    public FGAdType adType;
    public FGPlatform platform;

}

public enum FGAdType
{
    Interstitial, Rewarded, Banner
}

public enum FGPlatform
{
    IOS, Android
}
