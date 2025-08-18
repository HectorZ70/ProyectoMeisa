#if UNITY_EDITOR
using UnityEditor;
#endif
using SFB;
using System;
using System.IO;
using System.Text;
using UnityEngine;

public static class GridSaveLoad 
{
    public static void Save(GridSaveData data)
    {
        string path = StandaloneFileBrowser.SaveFilePanel("Guardar archivo", "", "", "json");

        if(string.IsNullOrEmpty(path))
        {
            Debug.Log("Guardado cancelado");
            return;
        }

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, json);

        Debug.Log($"Guardado en: {path}");
    }

    public static GridSaveData Load(string path)
    {
        if(!File.Exists(path))
        {
            Debug.LogWarning($"Archivo no encontrado: {path}");
            return null;
        }

        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<GridSaveData>(json);
    }

    public static GridSaveData LoadTSV()
    {
        var extensions = new[] {
        new ExtensionFilter("TSV Files", "tsv"),
    };

        string[] paths = StandaloneFileBrowser.OpenFilePanel("Abrir archivo", "", extensions, false);

        if (paths.Length == 0 || !File.Exists(paths[0]))
            return null;

        string[] lines = File.ReadAllLines(paths[0]);

        GridSaveData data = new GridSaveData();

        for (int i = 1; i < lines.Length; i++) // Saltar cabecera
        {
            string[] parts = lines[i].Split('\t');
            if (parts.Length < 4) continue;

            GridCellSaveData cell = new GridCellSaveData
            {
                row = int.Parse(parts[0]),
                column = int.Parse(parts[1]),
                text = parts[2],
                isHighlighted = bool.Parse(parts[3])
            };

            data.cells.Add(cell);
        }

        return data;
    }
}
