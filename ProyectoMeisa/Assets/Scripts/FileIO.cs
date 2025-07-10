using UnityEngine;
using System.IO;
using System.Collections.Generic;
using SFB;
using System;
using UnityEngine.UI;

public class FileIO : MonoBehaviour
{
    public VirtualizedGrid grid;         
    public TextAsset fileToLoad;         
    public bool isTSV = true;
    private bool isLoadingFile = false;

    private void Awake()
    {
        if (grid == null)
        {
            grid = FindFirstObjectByType<VirtualizedGrid>();
            if (grid == null)
                Debug.LogError("No se encontró ningún VirtualizedGrid en la escena.");
        }
    }

    public void OpenFile()
    {
        if (isLoadingFile) return;

        if (grid == null)
        {
            grid = FindFirstObjectByType<VirtualizedGrid>();
            if (grid == null)
            {
                Debug.LogError("grid sigue siendo null al abrir archivo.");
                return;
            }
        }

        isLoadingFile = true;

        var extensions = new[]
        {
        new ExtensionFilter("TSV files", "tsv"),
        new ExtensionFilter("CSV files", "csv"),
        new ExtensionFilter("All files", "*"),
    };

        StandaloneFileBrowser.OpenFilePanelAsync("Seleccionar archivo", "", extensions, false, (paths) =>
        {
            isLoadingFile = false;

            if (paths.Length > 0 && File.Exists(paths[0]))
            {
                string text = File.ReadAllText(paths[0]);
                this.isTSV = Path.GetExtension(paths[0]).ToLower() == ".tsv";
                LoadToGrid(text, this.isTSV);
            }
        });
    }

    public void LoadToGrid(string fileText, bool isTSV)
    {
        string[] lines = fileText.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

        int maxRow = 0;
        int maxCol = 0;

        for (int row = 0; row < lines.Length; row++)
        {
            char delimiter = DetectDelimiter(lines[row]);
            string[] cells = lines[row].Split(delimiter);

            for (int col = 0; col < cells.Length; col++)
            {
                grid.WriteToCell(row, col, cells[col]);
                maxCol = Mathf.Max(maxCol, col);
            }

            maxRow = Mathf.Max(maxRow, row);
        }

        grid.totalRows = Mathf.Max(grid.totalRows, maxRow + 1);
        grid.totalCols = Mathf.Max(grid.totalCols, maxCol + 1);
        grid.UpdateVisibleCells();
    }

    private char DetectDelimiter(string line)
    {
        if (line.Contains('\t')) return '\t';
        if (line.Contains(';')) return ';';
        if (line.Contains(',')) return ',';

        Debug.LogWarning("No se detectó delimitador claro en la línea: " + line);
        return ',';
    }
}