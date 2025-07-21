using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScrollableGrid : MonoBehaviour
{
    public ScrollRect scrollRect;
    public RectTransform content;
    public GameObject inputFieldPrefab;
    public ScrollRect linkedVerticalScrollRect;
    public ScrollRect linkedHorizontalScrollRect;

    public int totalRows = 1000;
    public int totalDateCols = 1000;
    public float cellWidth = 250f;
    public float cellHeight = 50f;

    public DateTime fechaInicial = new(2025, 7, 1);

    Dictionary<Vector2Int, string> data = new();
    Dictionary<Vector2Int, TMP_InputField> activeCells = new();

    bool syncing = false;

    void Start()
    {
        fechaInicial = DateTime.Today;

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

    public Dictionary<int, DateTime> GetVisibleDateColumns()
    {
        Dictionary<int, DateTime> visibleDates = new();

        foreach (var kvp in activeCells)
        {
            Vector2Int coord = kvp.Key;
            TMP_InputField cell = kvp.Value;

            if (coord.x == 0 && DateTime.TryParse(cell.text, out DateTime date))
            {
                visibleDates[coord.y - 2] = date;
            }
        }

        return visibleDates;
    }

    void UpdateVisibleCells()
    {
        Vector2 pos = content.anchoredPosition;
        float posX = -pos.x;
        float posY = Mathf.Max(0, pos.y);

        float viewWidth = scrollRect.viewport.rect.width;
        float viewHeight = scrollRect.viewport.rect.height;

        int startRow = Mathf.FloorToInt(posY / cellHeight);
        int visibleRows = Mathf.CeilToInt(viewHeight / cellHeight) + 1;

        int startCol = Mathf.FloorToInt(posX / cellWidth);
        int visibleCols = Mathf.CeilToInt(viewWidth / cellWidth) + 1;

        foreach (var cell in activeCells.Values)
            Destroy(cell.gameObject);
        activeCells.Clear();

        for (int r = 0; r < visibleRows; r++)
        {
            int row = startRow + r;

            for (int c = 0; c < visibleCols; c++)
            {
                int col = startCol + c;
                if (col >= totalDateCols + 2) continue;

                Vector2Int coord = new(row, col);

                GameObject go = Instantiate(inputFieldPrefab, content);
                TMP_InputField input = go.GetComponent<TMP_InputField>();

                if (row == 0)
                {
                    input.interactable = false;
                    DateTime fecha = fechaInicial.AddDays(col - 2);
                    input.text = fecha.ToString("dd/MM/yyyy");
                    input.textComponent.alignment = TextAlignmentOptions.Center;
                }
                else
                {
                    input.text = data.ContainsKey(coord) ? data[coord] : "";

                    int capturedRow = row;
                    int capturedCol = col;
                    input.onEndEdit.AddListener(value =>
                    {
                        data[new Vector2Int(capturedRow, capturedCol)] = value;
                    });
                }

                RectTransform rt = go.GetComponent<RectTransform>();
                rt.anchorMin = rt.anchorMax = new Vector2(0, 1);
                rt.pivot = new Vector2(0, 1);
                rt.anchoredPosition = new Vector2((col - 2) * cellWidth, -row * cellHeight);
                rt.sizeDelta = new Vector2(cellWidth, cellHeight);

                activeCells[coord] = input;
            }
        }
        content.sizeDelta = new Vector2(totalDateCols * cellWidth, totalRows * cellHeight);
    }
}
