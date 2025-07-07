using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using System;

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

    private float cellWidth = 250f;
    private float cellHeight = 50f;
    private Color highlightColor = Color.yellow;

    private Dictionary<Vector2Int, string> cellData = new(); 
    private Dictionary<Vector2Int, TMP_InputField> activeCells = new(); 
    private HashSet<int> highlightedColumns = new HashSet<int>();
    bool syncing = false; 

    void Start()
    {
        content.sizeDelta = new Vector2(totalCols * cellWidth, totalRows * cellHeight);



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
    void UpdateVisibleCells()
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