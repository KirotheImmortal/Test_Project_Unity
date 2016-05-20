using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
public class BuildManager : EditorWindow
{
    /// <summary>
    /// Stores The Scenes saved into build settings
    /// </summary>
    static List<string> m_Scenes = new List<string>();
    [MenuItem("Tools/BuildManager")]
    static void Init()
    {
        BuildManager window = GetWindow(typeof(BuildManager)) as BuildManager;
        window.Show();

        ///Gets the string name for each of the scens saved into the build settings
        foreach (EditorBuildSettingsScene a in UnityEditor.EditorBuildSettings.scenes)
        {
            if (a.enabled)
            {
                string tmp = a.path.Substring(a.path.LastIndexOf('/') + 1);

                if (!m_Scenes.Contains(tmp))
                    m_Scenes.Add(tmp);
            }
        }
    }

    void OnGUI()
    {
        if(GUILayout.Button("Build"))
        { Build(); }
    }

    void Build()
    {
        if (!System.IO.Directory.Exists(@".\Builds"))
            System.IO.Directory.CreateDirectory(@".\Builds");

        //UnityEditor.EditorBuildSettings.scenes
      

        BuildPipeline.BuildPlayer(m_Scenes.ToArray(), @".\Builds", EditorUserBuildSettings.activeBuildTarget, BuildOptions.AutoRunPlayer);
            
    }
    void LoadScene()
    {

    }
    
}
