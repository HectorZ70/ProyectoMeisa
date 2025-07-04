using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FixedGrid : MonoBehaviour
{
    public ScrollRect scrollRect;
    public RectTransform content;
    public GameObject inputFieldPrefab;

    public int totalRows = 100;
    public float cellWidth = 150f;
    public float cellHeight = 50f;

    Dictionary<Vector2Int, string> data = new();
    Dictionary<Vector2Int, TMP_InputField> activeCells = new();

    void Start()
    {
        scrollRect.onValueChanged.AddListener(_ => UpdateVisibleCells());
        UpdateVisibleCells();
    }

    void UpdateVisibleCells()
    {
        float contentY = Mathf.Max(0, content.anchoredPosition.y);
        float viewportHeight = scrollRect.viewport.rect.height;

        int startRow = Mathf.FloorToInt(contentY / cellHeight);
        int visibleRows = Mathf.CeilToInt(viewportHeight / cellHeight) + 1;

        // Siempre columnas 0 y 1
        int[] columnasFijas = { 0, 1 };

        foreach (var cell in activeCells.Values)
            Destroy(cell.gameObject);
        activeCells.Clear();

        foreach (int col in columnasFijas)
        {
            for (int r = 0; r < visibleRows; r++)
            {
                int row = startRow + r;
                Vector2Int coord = new(row, col);

                GameObject go = Instantiate(inputFieldPrefab, content);
                TMP_InputField input = go.GetComponent<TMP_InputField>();
                input.text = data.ContainsKey(coord) ? data[coord] : "";

                if (row == 0)
                {
                    input.interactable = false;
                    input.text = col == 0 ? "Nombre" : "Número";
                }
                else
                {
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
                rt.anchoredPosition = new Vector2(col * cellWidth, -row * cellHeight);
                rt.sizeDelta = new Vector2(cellWidth, cellHeight);

                activeCells[coord] = input;
            }
        }

        content.sizeDelta = new Vector2(2 * cellWidth, totalRows * cellHeight);
    }
}
