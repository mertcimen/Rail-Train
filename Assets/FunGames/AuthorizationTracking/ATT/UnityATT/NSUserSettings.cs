#if UNITY_EDITOR
using System.IO;

using UnityEditor.Callbacks;
using UnityEditor;

using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif

namespace FunGames.AuthorizationTracking.ATT.UnityATT
{
    /// <summary>
    /// PostProcessor script to automatically fill all required dependencies
    /// for App Tracking Transparency
    /// </summary>
    public class AppTrackingTransparencyPostProcessor
    {
#if UNITY_IOS
        [PostProcessBuild]
        public static void OnPostprocessBuild(BuildTarget buildTarget, string buildPath)
        {
            if (buildTarget == BuildTarget.iOS)
            {
                /*
                 * PBXProject
                 */
                PBXProject project = new PBXProject();
                string projectPath = PBXProject.GetPBXProjectPath(buildPath);
                project.ReadFromFile(projectPath);

                // If loaded, add `AppTrackingTransparency` Framework
                if (project != null)
                {
                    string targetId;
#if UNITY_2019_3_OR_NEWER
                    targetId = project.GetUnityFrameworkTargetGuid();
#else
                    targetId = project.TargetGuidByName("Unity-iPhone");
#endif

                    project.AddFrameworkToProject(targetId, "AppTrackingTransparency.framework", true);
                    project.AddFrameworkToProject(targetId, "AdSupport.framework", false);
                    project.AddFrameworkToProject(targetId, "StoreKit.framework", false);

                    project.WriteToFile(PBXProject.GetPBXProjectPath(buildPath));
                }

                /*
                 * PList
                 */
                PlistDocument plist = new PlistDocument();
                plist.ReadFromString(File.ReadAllText(buildPath + "/Info.plist"));
                if (plist != null)
                {
                    // Get root
                    PlistElementDict rootDict = plist.root;

                    // Add NSUserTrackingUsageDescription
                    rootDict.SetString("NSUserTrackingUsageDescription", SettingsInspector.Settings.PopupMessage);

                    // Check if SKAdNetworkItems already exists
                    PlistElementArray SKAdNetworkItems = null;
                    if (rootDict.values.ContainsKey("SKAdNetworkItems"))
                    {
                        try
                        {
                            SKAdNetworkItems = rootDict.values["SKAdNetworkItems"] as PlistElementArray;
                        }
                        catch (Exception e)
                        {
                            Debug.LogWarning(string.Format("Could not obtain SKAdNetworkItems PlistElementArray: {0}", e.Message));
                        }
                    }

                    // If not exists, create it
                    if (SKAdNetworkItems == null)
                    {
                        SKAdNetworkItems = rootDict.CreateArray("SKAdNetworkItems");
                    }

                    if (SettingsInspector.Settings.SkAdNetworkIds != null)
                    {
                        List<string> networkIdsWithoutDuplicates = SettingsInspector.Settings.SkAdNetworkIds.Distinct().ToList();
                        string plistContent = File.ReadAllText(buildPath + "/Info.plist");
                        for (int i = 0; i < networkIdsWithoutDuplicates.Count; i++)
                        {
                            if (!plistContent.Contains(networkIdsWithoutDuplicates[i]))
                            {
                                PlistElementDict SKAdNetworkIdentifierDict = SKAdNetworkItems.AddDict();
                                SKAdNetworkIdentifierDict.SetString("SKAdNetworkIdentifier", networkIdsWithoutDuplicates[i]);
                            }
                        }
                    }

                    File.WriteAllText(buildPath + "/Info.plist", plist.WriteToString());
                }
            }
        }
#endif
    }
}
#endif