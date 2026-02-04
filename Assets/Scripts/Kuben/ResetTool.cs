using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ResetTool
{
    #if UNITY_EDITOR
    [MenuItem("Tools/Clear All Save Data")]
    #endif
    public static void ClearAllData()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("<color=red><b>[ResetTool] ALL SAVE DATA DELETED!</b></color>");
    }
}