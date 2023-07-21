#if !DISABLE_SRDEBUGGER
using UnityEngine;

public class SRDebuggerInitializer
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void RunTimeInitialize()
    {
        SRDebug.Init();
        SRDebug.Instance.AddOptionContainer(GameSROptions.Instance);
    }
}
#endif