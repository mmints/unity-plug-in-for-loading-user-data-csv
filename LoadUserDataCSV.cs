using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Linq;
using UnityEngine.UIElements;

public class UserData
{
    private float TimeStamp { get; set; }
    private Vector3 Position { get; set; }
    private Vector3 LookAt { get; set; }
    
    public UserData(float timeStamp, Vector3 position, Vector3 lookAt)
    {
        TimeStamp = timeStamp;
        Position = position;
        LookAt = lookAt;
    }
}

public class LoadUserDataCSV : EditorWindow
{
    private string debugMessage = "";
    private string pathToCSV;

    private UserData[] userData = new UserData[0];

    // *** BEGIN GUI LAYOUT *** //
    
    [MenuItem("Window/Load User Data CSV")] // Add menu item to the Window menu
    public static void ShowWindow()
    {
        // Show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(LoadUserDataCSV));
    }
    
    void OnGUI()
    {
        pathToCSV = EditorGUILayout.TextField ("Path to CSV", pathToCSV);

        if (GUILayout.Button("Load"))
        {
            string data = File.ReadAllText(pathToCSV);
            string[] rows = data.Split('\n');

            // Remove first Row that contains the Headers of the CSV
            rows = rows.Skip(1).ToArray();
            rows = rows.Take(rows.Count() - 1).ToArray();
            
            debugMessage = rows.Length.ToString();

            foreach (string line in rows)
            {
                string[] fields = line.Split(';');
                
                Vector3 position = new Vector3(float.Parse(fields[1]), float.Parse(fields[2]), float.Parse(fields[3]));
                Vector3 lookAt = new Vector3(float.Parse(fields[4]), float.Parse(fields[5]), float.Parse(fields[6]));

                UserData ud = new UserData(float.Parse(fields[0]), position, lookAt);
                userData.Append(ud);
            }
            debugMessage = "Load the selected CSV into User Data Object.";
        }
        
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
