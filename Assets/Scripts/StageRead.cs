using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.IO;
public class StageRead : MonoBehaviour
{
    public string StageText;

    public class ListType
    {
        public char type;
        public List<Vector2> point;
    }

    public List<object> FileRead(string name)
    {
        string FilePath = Path.Combine(Application.streamingAssetsPath, $"{name}.lvl");
        string[] StageLines = File.ReadAllLines(FilePath);
        List<object> Stage = new List<object>{};
        List<object> StageRoute = new List<object>{};
        List<object> Route = new List<object>{};
        for (int x = 0; x < StageLines.Length; x++)
        {
            string StageText = StageLines[x];
            int index = StageText.IndexOf("#");
            if (index >= 0)
            {
                StageText = StageText.Substring(0, index);
            }
            if (x == 0 || x == 1)
            {
                Stage.Add(StageText);
            }
            else
            {
                string[] parts = StageText.Split(" ");
                for (int i = 0; i < parts.Length; i++)
                {
                    if (i == 0)
                    {
                        Route.Add((int)parts[i])
                    }
                    else if (i == 1 || i == 2)
                    {
                        Route.Add((string)parts[i])
                    }
                    else
                    {
                        var MatchText = Regex.Matches(parts[i], @"([A-Z])\{([^}]+)\}");
                        foreach (Match m in MatchText)
                        {
                            ListType cmd = new ListType();
                            cmd.type = m.Groups[1].Value[0];
                            cmd.points = new List<Vector2>();
                            var pointMatches = Regex.Matches(m.Groups[2].Value, @"\((-?\d+),\s*(-?\d+)\)");
                            foreach (Match p in pointMatches)
                            {
                                float x = float.Parse(p.Groups[1].Value);
                                float y = float.Parse(p.Groups[2].Value);
                                cmd.points.Add(new Vector2(x, y));
                            }
                            Route.Add(cmd);
                        }
                    }
                }
            }
            StageRoute.Add(Stage);
            StageRoute.Add(Route);
        }
        return StageRoute;
    }
}
