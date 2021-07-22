using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LoadUserDataCSV : EditorWindow
{
    private string debugMessage = "";
    
    // *** BEGIN GUI LAYOUT *** //
    
    [MenuItem("Window/Load User Data CSV")] // Add menu item to the Window menu
    public static void ShowWindow()
    {
        // Show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(LoadUserDataCSV));
    }

    void OnGUI()
    {
        if (GUILayout.Button("Show Data"))
        {
            debugMessage = "TODO: Visualize Data in Editor Window";
        }

        if (GUILayout.Button("Hide Data"))
        {
            debugMessage = "TODO: Stop Visualization of Data in Editor Window";
        }
        
        EditorGUILayout.Space();
        
        // Editor Window Debug Output
        EditorGUILayout.HelpBox(debugMessage, MessageType.Warning);
    }
}
