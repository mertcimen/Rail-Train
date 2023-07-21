using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using FunGames.Tools;
using FunGames.Tools.Core.EditorWindow;
using FunGames.Tools.Debugging;
using FunGames.Tools.Debugging.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public class IntegrationManagerController
{
    public RemoteDataHolder Data => _remoteDataHolder;

    private RemoteDataHolder _remoteDataHolder;

    private Dictionary<string, FGPackage> _packages = new Dictionary<string, FGPackage>();

    private Dictionary<string, List<FGModuleInfo>>
        _modulesByType = new Dictionary<string, List<FGModuleInfo>>();

    private Dictionary<string, FGModuleInfo> _modulesByName = new Dictionary<string, FGModuleInfo>();

    private Dictionary<string, ModuleViewer> _moduleVersions = new Dictionary<string, ModuleViewer>();

    private const string MAIN_LIST_URL = "1MxCkKaLsbSbOb_0wZwQbN0Vyy6JaU7Ds";
    // private const string SOFTLAUNCH_LIST_URL = "1cVfMgJsGQQeG2Mnh3QT_yj44A_P2CMXv";

    private const string DOWNLOAD_FILES_URL = "https://drive.google.com/uc?export=download&id=";
    private const string BASE_FILES_URL = "https://www.googleapis.com/drive/v3/files/";
    private const string BASE_FILES_URL_KEY = "?alt=media&key=AIzaSyBo_2y3JCRxroZsuSrIaFwcXC4AXIZ1IGQ";
    private const string REMOTE_DATA_FILE = "/RemoteModules.json";
    private int _requestedModules = 0;
    private int _loadedModules = 0;

    private string RemoteDataFile => Application.persistentDataPath + REMOTE_DATA_FILE;

    public void Initialize()
    {
        Debug.Log(RemoteDataFile);
        if (!IsCacheUpToDate())
        {
            _remoteDataHolder = new RemoteDataHolder(DateTime.Now);
            LoadRemoteData();
        }
        FGDebugSettings.settings.Version = FGToolsPackage.Package.GetModuleVersion();
    }

    private bool IsCacheUpToDate()
    {
        if (!File.Exists(RemoteDataFile)) return false;

        if (_remoteDataHolder == null)
        {
            _remoteDataHolder = JsonUtility.FromJson<RemoteDataHolder>(File.ReadAllText(RemoteDataFile));
            MapData();
        }

        DateTime lastUpdateDate = Convert.ToDateTime(_remoteDataHolder.LastUpdateDate, CultureInfo.InvariantCulture);
        return isUpToDate(lastUpdateDate);
    }

    private void LoadRemoteData()
    {
        // EditorWebRequest.SendRequest(SimpleWebRequestBuilder(SOFTLAUNCH_LIST_URL), LoadSoftLaunchModules);
        EditorWebRequest.SendRequest(SimpleWebRequestBuilder(MAIN_LIST_URL), LoadAllURLs);
    }

    private void AddModuleToParentDictionnary(FGModuleInfo module, string parent)
    {
        if (!_modulesByType.ContainsKey(parent)) _modulesByType.Add(parent, new List<FGModuleInfo>());
        _modulesByType[parent].Add(module);
    }

    private void AddModuleToDictionnary(FGModuleInfo module)
    {
        if (!_modulesByName.ContainsKey(module.Name)) _modulesByName.Add(module.Name, module);
        if (!_packages.ContainsKey(module.Name)) _packages.Add(module.Name, null);
    }

    private void LoadAllURLs(UnityWebRequest req)
    {
        IntegrationManager integrationManager = JsonUtility.FromJson<IntegrationManager>(req.downloadHandler.text);
        for (int i = 0; i < integrationManager.ModuleURLs.Count; i++)
        {
            var moduleURLs = integrationManager.ModuleURLs[i];
            AddTypeToCache(moduleURLs.Type);
            foreach (var url in moduleURLs.URLs)
            {
                _requestedModules++;
                EditorWebRequest.SendRequest(SimpleWebRequestBuilder(url), (req) => LoadModule(req, moduleURLs.Type));
            }
        }
    }

    // private void LoadSoftLaunchModules(UnityWebRequest req)
    // {
    //     SoftLaunchModules softLaunchModules = JsonUtility.FromJson<SoftLaunchModules>(req.downloadHandler.text);
    //     _remoteDataHolder.SoftLaunchModules = softLaunchModules;
    //     UpdateCacheFile();
    // }

    private void LoadModule(UnityWebRequest req, string type)
    {
        _loadedModules++;
        FGModuleInfo module = JsonUtility.FromJson<FGModuleInfo>(req.downloadHandler.text);
        AddModuleToCache(module, type);
        // Debug.Log("Success " + module.Name);
        if (_requestedModules.Equals(_loadedModules))
        {
            // Debug.Log("All modules have been loaded !");
            UpdateCacheFile();
            FGDebugSettings.settings.Version = FGToolsPackage.Package.GetModuleVersion();
        }
    }

    private void AddTypeToCache(string type)
    {
        bool typeExists = false;
        foreach (var moduleList in _remoteDataHolder.AllModules)
        {
            if (moduleList.Type.Equals(type))
            {
                typeExists = true;
                break;
            }
        }

        if (!typeExists) _remoteDataHolder.AllModules.Add(new ModuleListHolder(type));
    }

    private void AddModuleToCache(FGModuleInfo module, string type)
    {
        MapModule(module, type);
        foreach (var moduleList in _remoteDataHolder.AllModules)
        {
            if (!moduleList.Type.Equals(type)) continue;
            moduleList.ModuleList.Add(module);
        }
    }

    private void UpdateCacheFile()
    {
        File.WriteAllText(RemoteDataFile, JsonUtility.ToJson(_remoteDataHolder));
    }

    public void MapData()
    {
        foreach (var moduleList in _remoteDataHolder.AllModules)
        {
            foreach (var module in moduleList.ModuleList)
            {
                MapModule(module, moduleList.Type);
            }
        }
    }

    private void MapModule(FGModuleInfo module, string type)
    {
        AddModuleToParentDictionnary(module, type);
        AddModuleToDictionnary(module);
        GetAllPackages();
        UpdateModelVersions();
    }

    private static string getDownloadedPackage(FGModuleInfo module)
    {
        return Path.Combine(Application.dataPath, "AssetLibrary", "Package", module.Name + ".unitypackage");
    }

    public string GetCurrentInstalledVersion(FGModuleInfo module)
    {
        return isModuleInstalled(module)
            ? _moduleVersions[module.Name].localVersion
            : "not installed";
    }

    public string GetLasUpToDateVersion(FGModuleInfo module)
    {
        return _moduleVersions[module.Name].remoteVersion;
    }

    public bool isModuleUpToDate(FGModuleInfo module)
    {
        string localVersion = GetCurrentInstalledVersion(module);
        string remoteVersion = GetLasUpToDateVersion(module);
        return localVersion.Equals(remoteVersion);
    }

    public bool isModuleInstalled(FGModuleInfo module)
    {
        return GetPackage(module) != null;
    }

    private bool isUpToDate(DateTime date)
    {
        return Math.Abs(date.Subtract(DateTime.Now).TotalMinutes)  <= 30;
        // return date.DayOfYear.Equals(DateTime.Today.DayOfYear) && date.Year.Equals(DateTime.Today.Year);
    }

    public List<T> GetEnumerableOfType<T>(params object[] constructorArgs) where T : class //, IComparable<T>
    {
        List<T> objects = new List<T>();
        foreach (Type type in
            Assembly.GetAssembly(typeof(T)).GetTypes()
                .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(T))))
        {
            objects.Add((T)Activator.CreateInstance(type, constructorArgs));
        }

        return objects;
    }

    private FGPackage GetPackage(FGModuleInfo module)
    {
        return _packages[module.Name];
    }

    private void GetAllPackages()
    {
        List<FGPackage> packages = GetEnumerableOfType<FGPackage>();
        foreach (FGPackage package in packages)
        {
            if (String.IsNullOrEmpty(package.ModuleName)) continue;
            if(!_packages.ContainsKey(package.ModuleName)) _packages.Add(package.ModuleName, package);
            if (_packages[package.ModuleName] == null) _packages[package.ModuleName] = package;
        }
    }

    // public void DownLoadSoftLaunchModules()
    // {
    //     foreach (var moduleName in _remoteDataHolder.SoftLaunchModules.ModuleNames)
    //     {
    //         if (_modulesByName[moduleName] == null)
    //         {
    //             Debug.LogError("Missing package for SoftLaunch");
    //             continue;
    //         }
    //         
    //         DownloadAndInstall(_modulesByName[moduleName], false);
    //     }
    // }

    // public void AddSoftLaunchPrefabsAndSettings()
    // {
    //     foreach (var moduleName in _remoteDataHolder.SoftLaunchModules.ModuleNames)
    //     {
    //         _packages[moduleName]?.AddPrefabsAndSettings();
    //     }
    // }
    
    public void AddAllPrefabsAndSettings()
    {
        foreach (var package in _packages.Values)
        {
            package?.AddPrefabsAndSettings();
        }
    }

    public void AddPrefabsAndSettings(FGModuleInfo module)
    {
        if(GetPackage(module)==null) GetAllPackages();
        GetPackage(module).AddPrefabsAndSettings();
    }

    private void ImportPackage(UnityWebRequest req, FGModuleInfo module, bool interactive = true)
    {
        if (req.result == UnityWebRequest.Result.Success)
        {
            AssetDatabase.ImportPackage(getDownloadedPackage(module), interactive);
            GetPackage(module)?.UpdateSettings();
        }
        else
        {
            Debug.LogWarning("Package " + module.Name + " couldn't be downloaded !");
        }
    }

    private void UpdateModelVersions()
    {
        foreach (var module in _modulesByName.Values)
        {
            if (!_moduleVersions.ContainsKey(module.Name)) _moduleVersions.Add(module.Name, new ModuleViewer());
            _moduleVersions[module.Name].localVersion = GetPackage(module)?.GetModuleVersion();
            _moduleVersions[module.Name].remoteVersion = module.Version;
        }
    }
    
    private UnityWebRequest[] SimpleWebRequestBuilder(string url)
    {
        UnityWebRequest[] requestBuffer =
        {
            EditorWebRequest.SimpleRequest(BASE_FILES_URL + url + BASE_FILES_URL_KEY),
            // EditorWebRequest.SimpleRequest(DOWNLOAD_FILES_URL + url),
        };
        return requestBuffer;
    }

    // private UnityWebRequest[] DownloadFileWebRequestBuilder(FGModuleInfo module)
    // {
    //     UnityWebRequest[] requestBuffer =
    //     {
    //         EditorWebRequest.DownloadFileRequest(BASE_FILES_URL + module.PackageURL + BASE_FILES_URL_KEY,
    //             getDownloadedPackage(module)),
    //         // EditorWebRequest.DownloadFileRequest(DOWNLOAD_FILES_URL + module.PackageURL, getDownloadedPackage(module)),
    //     };
    //     return requestBuffer;
    // }
    

    public void DownloadAndInstall(FGModuleInfo module, bool interactive)
    {
        // EditorWebRequest.SendRequest(DownloadFileWebRequestBuilder(_modulesByName[moduleName]),
        //     (request => ImportPackage(request, _modulesByName[moduleName], false)));
        
        string url = BASE_FILES_URL + module.PackageURL + BASE_FILES_URL_KEY;
        string tempPath = Path.GetTempPath() + module.Name + ".unitypackage";
        // Download the package file from the URL
        UnityWebRequest request = UnityWebRequest.Get(url);
        request.SendWebRequest();

        int time = 0;
        while (!request.isDone)
        {
            time++;
            if (time >= 6E+8) break; //~30secs max by modules
        }

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.LogError("Error downloading package: " + request.error);
            return;
        }

        // Save the downloaded package file to a temporary location
        File.WriteAllBytes(tempPath, request.downloadHandler.data);

        // Import the package file
        AssetDatabase.ImportPackage(tempPath, interactive);

        GetPackage(module)?.UpdateSettings();
        // Delete the temporary file
        File.Delete(tempPath);
    }
}