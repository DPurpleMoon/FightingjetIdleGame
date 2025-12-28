using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class AttackPattern : MonoBehaviour
{
    public List<object> AttackRead(string name)
    {
        List<object> AttackPatternStyle = new List<object>{};
        List<float> ShotAngle = new List<float>{}; 
        string FilePath = Path.Combine(Application.streamingAssetsPath, $"{name}.txt");
        string Attack = File.ReadAllText(FilePath);
        string[] parts = StageText.Split(",");
        for (int i = 0; i < parts.Length; i++)
        {
            if (parts[i] == "")
            {
                ShotAngle.Add(0);
            } 
            else
            {
                ShotAngle.AddRange(parts[i].Trim('{', '}').Split(',').Select(float.Parse));
            }
            AttackPatternStyle.Add(ShotAngle);
        }
        return AttackPatternStyle;
    }
}