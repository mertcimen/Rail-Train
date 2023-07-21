using System.IO;
using System.Xml;
using UnityEngine;

public class AndroidManifestParser
{
    private const string ANDROID_MANIFEST_PATH = "Plugins/Android/AndroidManifest.xml";
    private static XmlDocument manifest;
    private static string appManifestPath;
    
    private static AndroidManifestParser instance = null;
    private static readonly object padlock = new object();

    AndroidManifestParser()
    {
        appManifestPath = Path.Combine(Application.dataPath, ANDROID_MANIFEST_PATH);

        if (!File.Exists(appManifestPath)) return;

        manifest = new XmlDocument();
        manifest.Load(appManifestPath);
    }

    public static AndroidManifestParser Instance
    {
        get
        {
            if (instance == null)
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new AndroidManifestParser();
                    }
                }
            }
            return instance;
        }
    }

    public void AddAllPermissions()
    {
        AddPermission(AndroidPermission.INTERNET);
        AddPermission(AndroidPermission.BIND_GET_INSTALL_REFERRER_SERVICE);
        AddPermission(AndroidPermission.AD_ID);
        AddPermission(AndroidPermission.ACCESS_NETWORK_STATE);
        AddPermission(AndroidPermission.ACCESS_WIFI_STATE);
        AddPermission(AndroidPermission.ADJUST_PREINSTALL);
    }

    public bool AddPermission(string permissionValue)
    {
        if (manifest == null)
        {
            Debug.LogWarning("No Android Manifest found at this location : " + appManifestPath);
            return false;
        }

        if (DoesPermissionExist(manifest, permissionValue))
        {
            Debug.Log(string.Format("[AndroidManifest]: Your app's AndroidManifest.xml file already contains {0} permission.",
                permissionValue));
            return false;
        }

        var element = manifest.CreateElement("uses-permission");
        AddAndroidNamespaceAttribute(manifest, "name", permissionValue, element);
        manifest.DocumentElement.AppendChild(element);
        Debug.Log(string.Format("[AndroidManifest]: {0} permission successfully added to your app's AndroidManifest.xml file.",
            permissionValue));
        return true;
    }

    private bool DoesPermissionExist(XmlDocument manifest, string permissionValue)
    {
        var xpath = string.Format("/manifest/uses-permission[@android:name='{0}']", permissionValue);
        return manifest.DocumentElement.SelectSingleNode(xpath, GetNamespaceManager(manifest)) != null;
    }

    private XmlNamespaceManager GetNamespaceManager(XmlDocument manifest)
    {
        var namespaceManager = new XmlNamespaceManager(manifest.NameTable);
        namespaceManager.AddNamespace("android", "http://schemas.android.com/apk/res/android");
        return namespaceManager;
    }

    private void AddAndroidNamespaceAttribute(XmlDocument manifest, string key, string value, XmlElement node)
    {
        var androidSchemeAttribute =
            manifest.CreateAttribute("android", key, "http://schemas.android.com/apk/res/android");
        androidSchemeAttribute.InnerText = value;
        node.SetAttributeNode(androidSchemeAttribute);
    }

    public static class AndroidPermission
    {
        public const string INTERNET = "android.permission.INTERNET";

        public const string BIND_GET_INSTALL_REFERRER_SERVICE =
            "com.google.android.finsky.permission.BIND_GET_INSTALL_REFERRER_SERVICE";

        public const string AD_ID = "com.google.android.gms.permission.AD_ID";
        public const string ACCESS_NETWORK_STATE = "android.permission.ACCESS_NETWORK_STATE";
        public const string ACCESS_WIFI_STATE = "android.permission.ACCESS_WIFI_STATE";
        public const string ADJUST_PREINSTALL = "com.adjust.preinstall.READ_PERMISSION";
    }
}