using System;

namespace FunGames.RemoteConfig
{
    [Serializable]
    public class FGRemoteConfigValue
    {
        public FGRemoteConfigValue(string name, string value)
        {
            this.name = name;
            this.value = value;
        }

        public string name;
        public string value;
    }
}