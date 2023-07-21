using UnityEngine;

namespace FunGames.Tools.Debugging
{
    public class FGDebugDefaultExample : MonoBehaviour
    {
        public void ClearConsole()
        {
            FGDebugConsole.Instance.Clear();
        }
    }
}