using System;

namespace FunGames.Tools
{
    public interface IFGModule
    {
        public string ModuleVersion { get; }
        public string ModuleName { get; }
        
        public float TimeForInitialization { get; }
        public bool IsInitialized();

        public bool MustBeInitialized();
    }
}