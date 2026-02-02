using UnityEngine;
using UnityEditor;

public class ResetTool : MonoBehaviour
{
    // Adds a menu item to the top bar of Unity: "Tools -> Clear Save Data"
    [MenuItem("Tools/Clear All Save Data")]
    public static void ClearAllData()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("<color=red><b>[ResetTool] ALL SAVE DATA DELETED!</b></color>");
    }
}