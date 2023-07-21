using FunGames.Monetization.IAP;
using UnityEngine;

public class FGUnityIAPExample : MonoBehaviour
{
    private void Start()
    {
        // StandardPurchasingModule.Instance().useFakeStoreAlways = true;
        // StandardPurchasingModule.Instance().useFakeStoreUIMode = FakeStoreUIMode.DeveloperUser;
    }

    public void BuyDiamondPack2()
    {
      FGIAP.BuyProduct("com.doublefun.moneyflow.diamond2");
    }
    
    public void BuyAssistant()
    {
        FGIAP.BuyProduct("com.doublefun.moneyflow.assistant");
    }
}