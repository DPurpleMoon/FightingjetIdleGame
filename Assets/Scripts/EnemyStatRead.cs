using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class EnemyStatRead : MonoBehaviour
{
    public List<object> EnemyStat(string name)
    {
        string FilePath = Path.Combine(Application.streamingAssetsPath, $"enemystat\\{name}.txt");
        string StatDetails = File.ReadAllText(FilePath);
        string[] parts = StatDetails.Split(" ");
        List<object> stats = new List<object>{};
        for (int i = 0; i < parts.Length; i++)
        {
            if (i == 0 || i > 1)
            {
                stats.Add(float.Parse(parts[i]));
            }
            else 
            {
                stats.Add(parts[i]);
            }
        }
        return stats;
    }

}

