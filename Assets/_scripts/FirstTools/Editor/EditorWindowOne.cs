using UnityEngine;
using UnityEditor;
using System;
public class EditorWindowOne : EditorWindow
{
    string myString = "Hello World";
    bool groupEnabled;
    bool myBool = true;
    float myFloat = 1.23f;
    float f;
    bool test;
    static string line = "Topics";
    
    [MenuItem ("Window/My Window")]//Adds a Menu Item to window as "My Window"
    public static void ShowWindow()
    {
        GetWindow(typeof(EditorWindowOne));//Show existing window instance. If one doesnt exist, make it.

    }


    void OnGUI()
    {
        line = null;
        line = "Strings beeing Listened for";
        foreach(string s in EventSystem.Subscriptions())
        {
            AddLineToMyWindow( s);
        }

        GUILayout.Label(line);

        

        //GUILayout.Label("Base Settings", EditorStyles.boldLabel);
        //myString = EditorGUILayout.TextField("Text Field", myString);
        //groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);
        //myBool = EditorGUILayout.Toggle("Toggle", myBool);
        //EditorGUILayout.Knob(new Vector2(100, 100), myFloat, -3, 3, "Test", Color.red, Color.blue ,myBool);
        //myFloat = EditorGUILayout.Slider("Slider", myFloat, -3, 3);
        //EditorGUILayout.Foldout(myBool, myBool.ToString() );
        //EditorGUILayout.EndToggleGroup();
        //EditorGUI.Label(new Rect(new Vector2(10, 10), new Vector2(0, 50)), "Test");
    }
    //[ContextMenuItem("Health","Health")]
    //private float heal;
    public static void AddLineToMyWindow(string s)
    {
        line += Environment.NewLine + s;
    }
    
}
