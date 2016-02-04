using UnityEngine;
using UnityEditor;

public class ToolOne : MonoBehaviour
{
    //@ContextMenu("Do Something");
   
    [MenuItem("Tools/Clear/PlayerPrefs")]
    private static void NewMenuOption()
    {
        print("Just some tool");
    }

    [MenuItem("Tools/SubMenu/Option %g")]
    private static void NewNestedOption()
    {
        print("Im dum");
    }
}
