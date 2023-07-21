using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Plugins.Tools.Editor
{
    /// <summary>
    /// Actions for command line build
    /// </summary>
    public class BuildActions
    {
        /// <summary>
        /// Get name from here or Config
        /// </summary>
        static string GAME_NAME = "***";

        private static int BUILD_NUMBER = 0;

        /// <summary>
        /// App version
        /// </summary>
        static string VERSION = Application.version + "_" + BUILD_NUMBER;

        /// <summary>
        /// Current project source path
        /// </summary>
        public static string APP_FOLDER = Directory.GetCurrentDirectory();

        /// <summary>
        /// iOS files path
        /// </summary>
        public static string IOS_FOLDER = string.Format("{0}/Builds/iOS/", APP_FOLDER);

        /// <summary>
        /// Get active scene list
        /// </summary>
        static string[] GetScenes()
        {
            List<string> scenes = new List<string>();
            for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
            {
                if (EditorBuildSettings.scenes[i].enabled)
                {
                    scenes.Add(EditorBuildSettings.scenes[i].path);
                }
            }

            return scenes.ToArray();
        }

        /// <summary>
        /// Run iOS release build
        /// </summary>
        static void iOSRelease()
        {
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.iOS, ScriptingImplementation.IL2CPP);
            // PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS, null);
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.iOS, BuildTarget.iOS);
            BuildReport report = BuildPipeline.BuildPlayer(GetScenes(), IOS_FOLDER, BuildTarget.iOS, BuildOptions.None);
            int code = (report.summary.result == BuildResult.Succeeded) ? 0 : 1;
            EditorApplication.Exit(code);
        }
    }
}