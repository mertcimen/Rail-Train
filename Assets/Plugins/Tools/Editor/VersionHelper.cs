using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Plugins.Tools.Editor
{
    public static class VersionHelper
    {
        [MenuItem("Tools/Build Version/Increase Major Version", false, 100)]
        private static void IncreaseMajor()
        {
            IncrementVersion(new[] { 1, 0, 0 });
        }

        [MenuItem("Tools/Build Version/Increase Minor Version", false, 100)]
        private static void IncreaseMinor()
        {
            IncrementVersion(new[] { 0, 1, 0 });
        }

        [MenuItem("Tools/Build Version/Increase Build Version", false, 100)]
        private static void IncreaseBuild()
        {
            IncrementVersion(new[] { 0, 0, 1 });
        }

        [MenuItem("Tools/Build Version/Increase Platforms Version (Android and iOS) " + "&v", false, 100)]
        private static void IncreasePlatformsVersion()
        {
            PlayerSettings.Android.bundleVersionCode += 1;
            PlayerSettings.iOS.buildNumber = (int.Parse(PlayerSettings.iOS.buildNumber) + 1).ToString();
            Debug.Log($"New Android bundle version code: {PlayerSettings.Android.bundleVersionCode}");
            Debug.Log($"New iOS build number: {PlayerSettings.iOS.buildNumber}");
        }

        private static void IncrementVersion(IList<int> version)
        {
            var lines = PlayerSettings.bundleVersion.Split('.');

            for (var i = lines.Length - 1; i >= 0; i--)
            {
                var isNumber = int.TryParse(lines[i], out var numberValue);

                if (!isNumber || version.Count - 1 < i)
                    continue;
                
                if (i > 0 && version[i] + numberValue > 9)
                {
                    version[i - 1]++;

                    version[i] = 0;
                }
                else
                {
                    version[i] += numberValue;
                }
            }

            PlayerSettings.bundleVersion = $"{version[0]}.{version[1]}.{version[2]}";
            Debug.Log($"New bundle version: {version[0]}.{version[1]}.{version[2]}");
        }
    }
}