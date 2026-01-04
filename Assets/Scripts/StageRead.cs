using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
public class StageRead : MonoBehaviour
{
    string StageText;
    int index;
    public List<object> FileRead(string name)
    {
        string FilePath = Path.Combine(Application.streamingAssetsPath, $"levels\\{name}.lvl");
        string[] StageLines = File.ReadAllLines(FilePath);
        List<object> StageRoute = new List<object>();
        for (int a = 0; a < StageLines.Length; a++)
        {
            StageText = StageLines[a];
            index = StageText.IndexOf("#");
            if (index >= 0)
            {
                StageText = StageText.Substring(0, index);
            }
            StageText = StageText.Trim();
            if (string.IsNullOrEmpty(StageText)) continue;
            if (a == 0 || a == 1)
            {
                StageRoute.Add(StageText);
            }
            else
            {
                List<object> Details = new List<object>();
                string[] parts = StageText.Split(" ");
                for (int i = 0; i < parts.Length; i++)
                {
                    //0 = y location, 1 = enemyname, 2 = movement speed, 3 = enemycount, 4 = bulletspeed
                    if (i == 0 || i == 2 || i == 4) 
                    {
                        Details.Add(float.Parse(parts[i]));
                    }
                    else if (i == 3)
                    {
                        Details.Add(int.Parse(parts[i]));
                    }
                    else if (i == 1)
                    {
                        Details.Add(parts[i].ToString());
                    }
                    else
                    {
                        var MatchText = Regex.Matches(parts[i], @"([A-Z])\{([^}]+)\}");
                        foreach (Match m in MatchText)
                        {
                            List<object> cmd = new List<object>();
                            cmd.Add(m.Groups[1].Value[0]);
                            var pointMatches = Regex.Matches(m.Groups[2].Value, @"\((-?\d+),\s*(-?\d+)\)");
                            foreach (Match p in pointMatches)
                            {
                                float x = float.Parse(p.Groups[1].Value);
                                float y = float.Parse(p.Groups[2].Value);
                                cmd.Add(new Vector2(x, y));
                            }
                            Details.Add(cmd);
                        }
                    }
                }
                StageRoute.Add(Details);
            }   
        }
        return StageRoute;
    }
}
