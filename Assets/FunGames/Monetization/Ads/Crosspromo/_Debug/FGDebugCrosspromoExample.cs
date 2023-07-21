using FunGames.Monetization.Crosspromo;
using UnityEngine;

public class FGDebugCrosspromoExample : MonoBehaviour
{
    public void DisplayCrossPromo()
    {
        FGCrosspromo.Play();
    }
    
    public void CloseCrossPromo()
    {
        FGCrosspromo.Close();
    }
}