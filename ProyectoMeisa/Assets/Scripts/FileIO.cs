using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System;

public class FileIO : MonoBehaviour
{
    public VirtualizedGrid grid;
    public TextAsset fileToLoad;
    public bool isTSV = true;

    void Start()
    {
        if (fileToLoad != null)
            LoadFromText(fileToLoad.text);
    }

    public void LoadFromText(string fileText)
    {
        string[] lines = fileText.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

        int maxRow = 0;
        int maxCol = 0;
        for (int row = 0; row < lines.Length; row++)
        {
            string[] cells = isTSV ? lines[row].Split('\t') : lines[row].Split(',');

            for (int col = 0; col < lines.Length; col++)
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
}
