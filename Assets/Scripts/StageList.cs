using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class StageList : MonoBehaviour
{
    void Start()
    {
        string streamingpath = Application.streamingAssetsPath;
        string[] filePaths = Directory.GetFiles($"{streamingpath}/levellist/", "*.json", SearchOption.AllDirectories);
        foreach (string filepath in filePaths)
        {
            
        string fileName = Path.GetFileName(filepath);
        int index = fileName.IndexOf(".json");
        if (index >= 0)
            {
                fileName = fileName.Substring(0, index);
            }
        
        int stagenum = int.Parse(fileName.Substring(5));    
        Debug.Log(stagenum);
        }
    }
    public void StagePressed()
    {
        SceneManager.LoadScene("Stage0");
    }

    public void ShopOpen()
    {
        SceneManager.LoadScene("Kuben");
    }
}
