using UnityEngine;
using System.IO;
using System.Collections.Generic;
using SFB;
using System;
using UnityEngine.UI;

public class FileIO : MonoBehaviour
{
    public VirtualizedGrid grid;         // Referencia a tu grilla
    public TextAsset fileToLoad;         // Opción de archivo precargado
    public bool isTSV = true;            // Tipo por defecto

    public void OpenFile()
    {
        var extensions = new[]
        {
            new ExtensionFilter("TSV files", "tsv"),
            new ExtensionFilter("CSV files", "csv"),
            new ExtensionFilter("All files", "*"),
        };

        // ?? El cuarto parámetro indica si se pueden seleccionar múltiples archivos (false = uno solo)
        string[] paths = StandaloneFileBrowser.OpenFilePanel("Seleccionar archivo", "", extensions, false);

        if (paths.Length > 0 && File.Exists(paths[0]))
        {
            string text = File.ReadAllText(paths[0]);

            // Detecta automáticamente el tipo por la extensión
            bool isTSV = Path.GetExtension(paths[0]).ToLower() == ".tsv";

            LoadToGrid(text, isTSV);
        }
    }

    public void LoadToGrid(string fileText, bool isTSV)
    {
        string[] lines = fileText.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

        int maxRow = 0;
        int maxCol = 0;

        for (int row = 0; row < lines.Length; row++)
        {
            string[] cells = isTSV ? lines[row].Split('\t') : lines[row].Split(',');

            for (int col = 0; col < cells.Length; col++)
            {
                grid.WriteToCell(row, col, cells[col]);
                maxCol = Mathf.Max(maxCol, col);
            }

            maxRow = Mathf.Max(maxRow, row);
        }

        // Ajusta el tamaño de la grilla virtual según lo que se cargó
        grid.totalRows = Mathf.Max(grid.totalRows, maxRow + 1);
        grid.totalCols = Mathf.Max(grid.totalCols, maxCol + 1);
        grid.UpdateVisibleCells();
    }
}