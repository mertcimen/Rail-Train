using System;
using FunGames.Tools.Core.EditorWindow;
using FunGames.Tools.Debugging.Editor;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class IntegrationManagerWindow : EditorWindow
{
    public GUISkin MainSkin;
    private Vector2 scrollPos = Vector2.zero;
    private float previousWindowWidth;
    private float w1;
    private float w2;
    private float w3;
    private float w4;
    private float w5;
    private float w6;
    private const float SPACING = 10;

    private static bool isInit = false;
    private static bool isViewUpdated = false;

    private static IntegrationManagerController _imc;

    [MenuItem("FunGames/Integration Manager")]
    public static void Init()
    {
        GetWindow<IntegrationManagerWindow>();
    }

    void OnEnable()
    {
        titleContent.text = "FunGames SDK";
        position = new Rect(200, 200, 800, 600);
        minSize = new Vector2(650, 400);
        previousWindowWidth = minSize.x;

        _imc = new IntegrationManagerController();
        _imc.Initialize();
    }

    void OnGUI()
    {
        if (Math.Abs(previousWindowWidth - position.width) > 1)
        {
            previousWindowWidth = position.width;
            w1 = position.width * 0.2f;
            w2 = position.width * 0.2f;
            w3 = position.width * 0.2f;
            w4 = position.width * 0.4f / 3;
            w5 = position.width * 0.4f / 2;
            w6 = position.width * 0.4f / 3;
        }
        
        GUI.skin = MainSkin;
        scrollPos = GUILayout.BeginScrollView(scrollPos, GUILayout.Width(this.position.width));
        // DrawSoftLaunch();
        DrawAll();
        DrawHeader();
        DrawAllModules();
        GUILayout.EndScrollView();
    }

    private void DrawHeader()
    {
        GUILayout.Space(SPACING);
        GUILayout.BeginHorizontal(EditorStyles.helpBox);
        GUILayout.Label("Module Name", EditorStyles.boldLabel, GUILayout.Width(w1));
        GUILayout.Label("Current Version", EditorStyles.boldLabel, GUILayout.Width(w2));
        GUILayout.Label("Last Version", EditorStyles.boldLabel, GUILayout.Width(w3));
        GUILayout.EndHorizontal();
    }

    // private void DrawSoftLaunch()
    // {
    //     GUILayout.Space(SPACING);
    //     GUILayout.BeginHorizontal(EditorStyles.helpBox);
    //     GUILayout.Label("Soft Launch", EditorStyles.boldLabel, GUILayout.Width(w1));
    //     GUILayout.Label("", GUILayout.Width(w2));
    //     GUILayout.Label("", GUILayout.Width(w3));
    //     if (GUILayout.Button("Import all", GUILayout.Width(w4)))
    //     {
    //         _imc.DownLoadSoftLaunchModules();
    //     }
    //
    //     if (GUILayout.Button("Prefabs & Settings", GUILayout.Width(w5)))
    //     {
    //         _imc.AddSoftLaunchPrefabsAndSettings();
    //     }
    //
    //     GUILayout.EndHorizontal();
    // }
    
    private void DrawAll()
    {
        GUILayout.Space(SPACING);
        GUILayout.BeginHorizontal(EditorStyles.helpBox);
        GUILayout.Label("All Modules", EditorStyles.boldLabel, GUILayout.Width(w1));
        GUILayout.Label("", GUILayout.Width(w2));
        GUILayout.Label("", GUILayout.Width(w3));
        GUILayout.Label("", GUILayout.Width(w4));

        if (GUILayout.Button("Prefabs & Settings", GUILayout.Width(w5)))
        {
            _imc.AddAllPrefabsAndSettings();
        }

        GUILayout.EndHorizontal();
    }

    private void DrawAllModules()
    {
        foreach (var moduleList in _imc.Data.AllModules) DrawType(moduleList);
    }

    private void DrawType(ModuleListHolder moduleList)
    {
        GUILayout.Space(SPACING);
        GUILayout.Label(moduleList.Type, EditorStyles.boldLabel);
        GUILayout.BeginVertical();
        foreach (var module in moduleList.ModuleList) DrawModule(module);
        GUILayout.EndVertical();
    }

    private void DrawModule(FGModuleInfo module)
    {
        GUILayout.BeginHorizontal();
        if (module.Name != FGPackageFolders.TOOLS) GUILayout.Label(module.Name, GUILayout.Width(w1));
        else GUILayout.Label("Main", GUILayout.Width(w1));

        GUILayout.Label(_imc.GetCurrentInstalledVersion(module), GUILayout.Width(w2));
        GUILayout.Label(_imc.GetLasUpToDateVersion(module), GUILayout.Width(w3));

        DrawInstallButton(module);
        DrawCreatePrefabAndSettingsButton(module);

        GUILayout.EndHorizontal();
    }

    private void DrawInstallButton(FGModuleInfo module)
    {
        bool upToDate = _imc.isModuleUpToDate(module);
        EditorGUI.BeginDisabledGroup(upToDate && _imc.isModuleInstalled(module));
        if (GUILayout.Button(GetInstallButtonLabel(module), GUILayout.Width(w4)))
        {
            Debug.Log("install : " + module.PackageURL);
            _imc.DownloadAndInstall(module, true);
        }

        EditorGUI.EndDisabledGroup();
    }

    private string GetInstallButtonLabel(FGModuleInfo module)
    {
        string label;
        if (!_imc.isModuleInstalled(module)) label = "Install";
        else if (!_imc.isModuleUpToDate(module)) label = "Update";
        else label = "Installed";

        return label;
    }

    private void DrawCreatePrefabAndSettingsButton(FGModuleInfo module)
    {
        EditorGUI.BeginDisabledGroup(!_imc.isModuleInstalled(module));
        if (module.Name != FGPackageFolders.TOOLS)
        {
            if (GUILayout.Button("Prefabs & Settings", GUILayout.Width(w5)))
            {
                _imc.AddPrefabsAndSettings(module);
                EditorSceneManager.SaveOpenScenes();
            }
        }

        EditorGUI.EndDisabledGroup();
    }
}