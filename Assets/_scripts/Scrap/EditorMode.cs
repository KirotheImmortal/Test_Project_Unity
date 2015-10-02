using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class EditorMode : MonoBehaviour
{

    public string hello;
    // Use this for initialization
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        print("Does stuff from : " + hello);
    }
}
