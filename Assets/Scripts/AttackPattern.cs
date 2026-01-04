using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class AttackPattern : MonoBehaviour
{
    public List<object> AttackRead(string name)
    {
        List<object> AttackPatternStyle = new List<object>{};
        string FilePath = Path.Combine(Application.streamingAssetsPath, $"attackpattern\\{name}.txt");
        string Attack = File.ReadAllText(FilePath);
        string[] parts = Attack.Split(",");
        for (int i = 0; i < parts.Length; i++)
        {
            List<float> ShotAngle = new List<float>{}; 
            if (parts[i] == "")
            {
                AttackPatternStyle.Add(0);
            } 
            else
            {
                string angle = parts[i];
                ShotAngle.AddRange(angle.Trim('{', '}').Split(';').Select(float.Parse));
            }
            AttackPatternStyle.Add(ShotAngle);
        }
        return AttackPatternStyle;
    }
}