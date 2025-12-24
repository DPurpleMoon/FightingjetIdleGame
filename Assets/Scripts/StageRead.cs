using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.IO;
public class StageRead : MonoBehaviour
{
    public string StageText;

    public List<object> FileRead(string name)
    {
        string FilePath = Path.Combine(Application.streamingAssetsPath, $"{name}.lvl");
        StageText = File.ReadAllText(filePath);
    }
}
