using System.IO;
using UnityEngine;

public static class GridSaveLoad
{
    private static readonly string path = Path.Combine(Application.persistentDataPath, "griddata.json");

    public static void Save(GridSaveData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, json);
    }

    public static GridSaveData Load()
    {
        if (!File.Exists(path))
        {
            return null;
        }
        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<GridSaveData>(json);
    }
}
