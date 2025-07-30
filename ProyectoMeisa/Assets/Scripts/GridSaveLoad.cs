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
#if UNITY_EDITOR
    public static void Save(GridSaveData data)
    {
        var extensions = new[] {
        new ExtensionFilter("TSV Files", "tsv"),
    };

        string path = StandaloneFileBrowser.SaveFilePanel("Guardar archivo", "", "griddata", "tsv");
        if (string.IsNullOrEmpty(path)) return;

        StringBuilder sb = new StringBuilder();
        sb.AppendLine("ro\tcolumn\ttext\tisHighlighted");

        foreach (var cell in data.cells)
        {
            sb.AppendLine($"{cell.row}\t{cell.column}\t{cell.text}\t{cell.isHighlighted}");
        }

        File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
    }
#endif

    public static GridSaveData Load()
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

    public static void SaveTSV(GridSaveData data, string path)
    {
        if (string.IsNullOrEmpty(path)) return;

        StringBuilder sb = new StringBuilder();
        sb.AppendLine("row\tcolumn\ttext\tisHighlighted");

        foreach (var cell in data.cells)
        {
            sb.AppendLine($"{cell.row}\t{cell.column}\t{cell.text}\t{cell.isHighlighted}");
        }

        File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
    }
}
