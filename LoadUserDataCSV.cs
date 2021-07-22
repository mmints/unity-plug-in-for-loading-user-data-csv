using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Linq;
using UnityEngine.UIElements;

public class UserData
{
    public float TimeStamp { get; set; }
    public Vector3 Position { get; set; }
    public Vector3 LookAt { get; set; }
    
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

    private List<UserData> userData = new List<UserData>();

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
                
                userData.Add(new UserData(float.Parse(fields[0]), position, lookAt));
            }
            debugMessage = "Load the selected CSV with this count of data point: " + userData.Count.ToString();
        }
        
        if (GUILayout.Button("View Position"))
        {
            ViewPosition();
        }
        
        
        if (GUILayout.Button("View Look At"))
        {
            ViewLookAt();
        }

        if (GUILayout.Button("Hide Data"))
        {
            debugMessage = "TODO: Stop Visualization of Data in Editor Window";
        }
        
        EditorGUILayout.Space();
        
        // Editor Window Debug Output
        EditorGUILayout.HelpBox(debugMessage, MessageType.Warning);
    }
    
    
    // Some Testing functions
    void ViewPosition()
    {
        Vector3 scaleChange = new Vector3(-0.01f, -0.01f, -0.01f);

        GameObject positionMarker = GameObject.CreatePrimitive(PrimitiveType.Cube);
        var positionMarkerRenderer = positionMarker.GetComponent<Renderer>();

        // FIXME: Instantiating material due to calling renderer.material during edit mode. This will leak materials into the scene. You most likely want to use renderer.sharedMaterial instead.
        positionMarkerRenderer.material.SetColor("_Color",Color.red);
        positionMarker.transform.localScale = scaleChange;

        foreach (var dataPoint in userData)
        {
            PlaceObject(positionMarker, dataPoint.Position);
        }
    }

    void ViewLookAt()
    {
        Vector3 scaleChange = new Vector3(-0.01f, -0.01f, -0.01f);
        
        GameObject lookAtMarker = GameObject.CreatePrimitive(PrimitiveType.Cube);
        var lookAtMarkerRenderer = lookAtMarker.GetComponent<Renderer>();
        
        // FIXME: Instantiating material due to calling renderer.material during edit mode. This will leak materials into the scene. You most likely want to use renderer.sharedMaterial instead.
        lookAtMarkerRenderer.material.SetColor("_Color",Color.green);
        lookAtMarker.transform.localScale = scaleChange;
        
        foreach (var dataPoint in userData)
        {
            PlaceObject(lookAtMarker, dataPoint.Position + dataPoint.LookAt);
        }
    }

    void PlaceObject(GameObject go, Vector3 position)
    {
        Instantiate(go, position, Quaternion.identity);
    }
}
