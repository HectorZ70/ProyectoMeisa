using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using System;
using System.Linq;

public class VirtualizedGrid : MonoBehaviour
{
    public RectTransform content;               
    public ScrollRect scrollRect;               
    public GameObject inputFieldPrefab;          
    public ScrollRect linkedVerticalScrollRect;
    public ScrollRect linkedHorizontalScrollRect;

    public int totalRows = 1000;
    public int totalCols = 1000;
    public int visibleRows = 20;                
    public int visibleCols = 10;        
    
    public Button guardarBtn, cargarBtn;

    public float cellWidth = 250f;
    public float cellHeight = 50f;
    private Color highlightColor = Color.yellow;

    private Dictionary<Vector2Int, string> cellData = new(); 
    private Dictionary<Vector2Int, TMP_InputField> activeCells = new(); 
    private HashSet<int> highlightedColumns = new HashSet<int>();
    bool syncing = false;
    public int rowCount;
    public int colCount;

    void Start()
    {
        content.sizeDelta = new Vector2(totalCols * cellWidth, totalRows * cellHeight);

        if (!string.IsNullOrEmpty(GridLoadBuffer.RawFileData))
        {
            LoadTSVFromBuffer();
            GridLoadBuffer.RawFileData = null; // Limpiar buffer
        }
        else if (GridLoadBuffer.DataToLoad != null)
        {
            LoadFromData(GridLoadBuffer.DataToLoad);
            GridLoadBuffer.DataToLoad = null;
        }

        if (GridLoadBuffer.DataToLoad != null)
        {
            LoadFromData(GridLoadBuffer.DataToLoad);
            GridLoadBuffer.DataToLoad = null; // Limpieza
        }

        scrollRect.onValueChanged.AddListener(_ =>
        {
            if (syncing) return;
            syncing = true;

            if (linkedVerticalScrollRect)
                linkedVerticalScrollRect.verticalNormalizedPosition = scrollRect.verticalNormalizedPosition;

            if (linkedHorizontalScrollRect)
                linkedHorizontalScrollRect.horizontalNormalizedPosition = scrollRect.horizontalNormalizedPosition;

            syncing = false;
        });

        if (linkedVerticalScrollRect)
        {
            linkedVerticalScrollRect.onValueChanged.AddListener(_ =>
            {
                if (syncing) return;
                syncing = true;
                scrollRect.verticalNormalizedPosition = linkedVerticalScrollRect.verticalNormalizedPosition;
                syncing = false;
            });
        }

        if (linkedHorizontalScrollRect)
        {
            linkedHorizontalScrollRect.onValueChanged.AddListener(_ =>
            {
                if (syncing) return;
                syncing = true;
                scrollRect.horizontalNormalizedPosition = linkedHorizontalScrollRect.horizontalNormalizedPosition;
                syncing = false;
            });
        }
        scrollRect.onValueChanged.AddListener(_ => UpdateVisibleCells());
        UpdateVisibleCells();
    }

    public void HighlightColumnsByDateRange(DateTime startDate, DateTime endDate, Dictionary<int, DateTime> dateColumns, Color highlightColor)
    {
        highlightedColumns.Clear();
        this.highlightColor = highlightColor;

        foreach (var kvp in dateColumns)
        {
            int col = kvp.Key;
            DateTime date = kvp.Value;

            if (date >= startDate && date <= endDate)
                highlightedColumns.Add(col);
        }

        UpdateHighlighting();
    }
    private void UpdateHighlighting()
    {
        foreach (var kvp in activeCells)
        {
            Vector2Int coord = kvp.Key;
            TMP_InputField cell = kvp.Value;

            if (highlightedColumns.Contains(coord.y))
                HighlightCell(cell);
            else
                ResetCellVisual(cell);
        }
    }

    private void HighlightCell(TMP_InputField cell)
    {
        if (cell.image != null)
            cell.image.color = highlightColor;
    }

    private void ResetCellVisual(TMP_InputField cell)
    {
        if (cell.image != null)
            cell.image.color = Color.white;
    }

    public void WriteToCell(int row, int col, string text)
    {
        Vector2Int coord = new(row, col);
        cellData[coord] = text;

        if (activeCells.TryGetValue(coord, out TMP_InputField cell))
        {
            cell.text = text;
        }
    }

    public void ClearCellsInColumns(List<int> columnsToClear)
    {
        foreach (var col in columnsToClear)
        {
            for (int row = 0; row < totalRows; row++)
            {
                Vector2Int coord = new(row, col);

                if (cellData.ContainsKey(coord))
                    cellData.Remove(coord);

                if (activeCells.TryGetValue(coord, out TMP_InputField cell))
                {
                    cell.text = "";
                    ResetCellVisual(cell);
                }
            }
        }

        foreach (var col in columnsToClear)
            highlightedColumns.Remove(col);

        UpdateHighlighting();
    }

    private void SyncVisibleCellsToData()
    {
        foreach (var kvp in activeCells)
        {
            Vector2Int coord = kvp.Key;
            TMP_InputField input = kvp.Value;
            cellData[coord] = input.text;
        }
    }

    public void SortGridByColumn(int colIndex, bool isNumeric)
    {
        Debug.Log("Llamando sort numérico\n" + Environment.StackTrace);
        SyncVisibleCellsToData();
        List<string[]> allRows = new List<string[]>();

        for (int row = 0; row < totalRows; row++)
        {
            string[] rowData = new string[totalCols];
            for (int col = 0; col < totalCols; col++)
            {
                Vector2Int coord = new Vector2Int(row, col);
                rowData[col] = cellData.TryGetValue(coord, out string value) ? value : "";
            }
            allRows.Add(rowData);
        }

        if (isNumeric)
        {
            allRows = allRows.OrderBy(row => int.TryParse(row[colIndex], out int n) ? n : int.MaxValue).ToList();
        }
        else
        {
            allRows = allRows.OrderBy(row => row[colIndex]).ToList();
        }

        cellData.Clear();
        for (int row = 0; row < allRows.Count; row++)
        {
            for (int col = 0; col < totalCols; col++)
            {
                Vector2Int coord = new Vector2Int(row, col);
                cellData[coord] = allRows[row][col];
            }
        }

        UpdateVisibleCells();
        Debug.Log("Sort numérico terminado");
    }

    public void SortBySecondColumnAlphabetically(bool ascending = true)
    {
        Debug.Log("Llamando sort alfabético");
        SyncVisibleCellsToData();

        List<string[]> allRows = new List<string[]>();

        for (int row = 0; row < totalRows; row++)
        {
            string[] rowData = new string[totalCols];
            for (int col = 0; col < totalCols; col++)
            {
                Vector2Int coord = new Vector2Int(row, col);
                rowData[col] = cellData.TryGetValue(coord, out string value) ? value : "";
            }
            allRows.Add(rowData);
        }

        allRows = ascending
            ? allRows.OrderBy(row => row.Length > 1 ? row[1] : "").ToList()
            : allRows.OrderByDescending(row => row.Length > 1 ? row[1] : "").ToList();

        cellData.Clear();                
        totalRows = allRows.Count;       

        for (int row = 0; row < allRows.Count; row++)
        {
            for (int col = 0; col < totalCols; col++)
            {
                cellData[new Vector2Int(row, col)] = allRows[row][col];
            }
        }

        UpdateVisibleCells();

        Debug.Log("Sort alfabético terminado");
    }

    public void Save()
    {
        GridSaveData saveData = new GridSaveData();

        foreach (var kvp in cellData)
        {
            Vector2Int coord = kvp.Key;
            string text = kvp.Value;

            bool isHighlighted = highlightedColumns.Contains(coord.y);

            GridCellSaveData data = new GridCellSaveData
            {
                row = coord.x,
                column = coord.y,
                text = text,
                isHighlighted = isHighlighted
            };

            saveData.cells.Add(data);
        }

        GridSaveLoad.Save(saveData);
    }

    public void Load()
    {
        var data = GridSaveLoad.Load();
        if (data != null)
        {
            GridLoadBuffer.DataToLoad = data;
            UnityEngine.SceneManagement.SceneManager.LoadScene("ListScene");
        }
        else
        {
            Debug.LogWarning("No se encontró un archivo válido para cargar.");
        }
    }

    public void LoadFromData(GridSaveData loaded)
    {
        cellData.Clear();
        highlightedColumns.Clear();

        foreach (var cellDataItem in loaded.cells)
        {
            Vector2Int coord = new(cellDataItem.row, cellDataItem.column);
            cellData[coord] = cellDataItem.text;

            if (cellDataItem.isHighlighted)
                highlightedColumns.Add(coord.y);
        }

        UpdateVisibleCells();
    }

    private void LoadTSVFromBuffer()
    {
        string[] lines = GridLoadBuffer.RawFileData.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

        int maxRow = 0;
        int maxCol = 0;

        for (int row = 0; row < lines.Length; row++)
        {
            char delimiter = DetectDelimiter(lines[row]);
            string[] cells = lines[row].Split(delimiter);

            for (int col = 0; col < cells.Length; col++)
            {
                WriteToCell(row, col, cells[col]);
                maxCol = Mathf.Max(maxCol, col);
            }

            maxRow = Mathf.Max(maxRow, row);
        }

        totalRows = Mathf.Max(totalRows, maxRow + 1);
        totalCols = Mathf.Max(totalCols, maxCol + 1);
        UpdateVisibleCells();
    }

    public string ReadFromCell(int row, int col)
    {
        Vector2Int coord = new Vector2Int(row, col);
        return cellData.TryGetValue(coord, out string value) ? value : "";
    }

    private char DetectDelimiter(string line)
    {
        if (line.Contains('\t')) return '\t';
        if (line.Contains(';')) return ';';
        if (line.Contains(',')) return ',';

        Debug.LogWarning("No se detectó delimitador claro en la línea: " + line);
        return ',';
    }


    public void UpdateVisibleCells()
    {

        float viewportWidth = scrollRect.viewport.rect.width;
        float viewportHeight = scrollRect.viewport.rect.height;


        float contentPosX = -content.anchoredPosition.x;
        float contentPosY = content.anchoredPosition.y;


        contentPosX = Mathf.Max(0, contentPosX);
        contentPosY = Mathf.Max(0, contentPosY);


        int startCol = Mathf.FloorToInt(contentPosX / cellWidth);
        int startRow = Mathf.FloorToInt(contentPosY / cellHeight);


        int visibleCols = Mathf.CeilToInt(viewportWidth / cellWidth) + 1;
        int visibleRows = Mathf.CeilToInt(viewportHeight / cellHeight) + 1;


        foreach (var cell in activeCells.Values)
            Destroy(cell.gameObject);
        activeCells.Clear();


        for (int r = 0; r < visibleRows; r++)
        {
            for (int c = 0; c < visibleCols; c++)
            {
                int row = startRow + r;
                int col = startCol + c;

                if (row >= totalRows || col >= totalCols) continue;

                Vector2Int coord = new(row, col);

                GameObject go = Instantiate(inputFieldPrefab, content);
                TMP_InputField input = go.GetComponent<TMP_InputField>();
                activeCells[coord] = input;

                input.text = cellData.ContainsKey(coord) ? cellData[coord] : "";

                int capturedRow = row;
                int capturedCol = col;

                input.onEndEdit.AddListener(value =>
                {
                    cellData[new Vector2Int(capturedRow, capturedCol)] = value;
                });

                RectTransform rt = go.GetComponent<RectTransform>();
                rt.anchorMin = rt.anchorMax = new Vector2(0, 1);
                rt.pivot = new Vector2(0, 1);
                rt.anchoredPosition = new Vector2(col * cellWidth, -row * cellHeight);
                rt.sizeDelta = new Vector2(cellWidth, cellHeight);


                if (highlightedColumns.Contains(col))
                    HighlightCell(input);
                else
                    ResetCellVisual(input);
            }
        }
        UpdateHighlighting();
    }
}