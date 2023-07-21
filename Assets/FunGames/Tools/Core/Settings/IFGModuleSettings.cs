using UnityEngine;

namespace FunGames.Tools.Debugging
{
    public abstract class IFGModuleSettings : ScriptableObject  
    {
        public abstract string Version { get; set; }
    }
}