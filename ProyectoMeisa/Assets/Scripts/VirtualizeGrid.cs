using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class VirtualizedGrid : MonoBehaviour
{
    public RectTransform content;                // Content del Scroll View
    public ScrollRect scrollRect;                // ScrollRect que controla el desplazamiento
    public GameObject inputFieldPrefab;          // Prefab de InputField

    public int totalRows = 1000;
    public int totalCols = 1000;
    public int visibleRows = 20;                 // Cuántas filas se ven en pantalla
    public int visibleCols = 10;                 // Cuántas columnas se ven en pantalla

    private float cellWidth = 250f;
    private float cellHeight = 50f;

    private Dictionary<Vector2Int, string> cellData = new(); // Guarda el contenido por celda
    private Dictionary<Vector2Int, TMP_InputField> activeCells = new(); // Celdas visibles

    void Start()
    {
        // Establece el tamaño del content
        content.sizeDelta = new Vector2(totalCols * cellWidth, totalRows * cellHeight);

        // Llama a UpdateVisibleCells cada vez que se hace scroll
        scrollRect.onValueChanged.AddListener(_ => UpdateVisibleCells());
        UpdateVisibleCells();
    }

    void UpdateVisibleCells()
    {
        // Posición actual del scroll (0 = arriba/izquierda, 1 = abajo/derecha)
        float normX = scrollRect.horizontalNormalizedPosition;
        float normY = scrollRect.verticalNormalizedPosition;

        // Calcula fila y columna superior izquierda visibles
        int startCol = Mathf.Clamp((int)(normX * (totalCols - visibleCols)), 0, totalCols - visibleCols);
        int startRow = Mathf.Clamp((int)((1 - normY) * (totalRows - visibleRows)), 0, totalRows - visibleRows);

        // Reutiliza celdas visibles
        foreach (var cell in activeCells.Values)
            Destroy(cell.gameObject);
        activeCells.Clear();

        // Crea nuevas celdas visibles
        for (int r = 0; r < visibleRows; r++)
        {
            for (int c = 0; c < visibleCols; c++)
            {
                int row = startRow + r;
                int col = startCol + c;
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
            }
        }
    }
}