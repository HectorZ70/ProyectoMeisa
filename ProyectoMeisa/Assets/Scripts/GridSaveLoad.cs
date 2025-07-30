#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using System.IO;
using System.Text;
using UnityEngine;

public static class GridSaveLoad 
{
#if UNITY_EDITOR
    public static void Save(GridSaveData data)
    {
        string path = EditorUtility.SaveFilePanel("Guardar como TSV", "", "griddata.tsv", "tsv");
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
}
