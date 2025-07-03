using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    public GameObject cellPrefab;  // Prefab con InputField (Legacy)
    public int rows = 10;
    public int columns = 10;

    private GridLayoutGroup gridLayout;

    void Start()
    {
        gridLayout = GetComponent<GridLayoutGroup>();
        if (gridLayout == null)
        {
            Debug.LogError("El GameObject debe tener un GridLayoutGroup.");
            return;
        }

        GenerateGrid();
    }

    public void ResizeGrid(int newRows, int newColumns)
    {
        rows = newRows;
        columns = newColumns;

        // Limpiar celdas viejas
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // Opcional: Ajustar el tamaño de celda para que encaje
        AdjustCellSize();

        GenerateGrid();
    }

    void AdjustCellSize()
    {
        // Ajustar el tamaño de las celdas según el tamaño del panel y la cantidad de filas y columnas
        RectTransform rt = GetComponent<RectTransform>();
        float width = rt.rect.width;
        float height = rt.rect.height;

        float cellWidth = width / columns - gridLayout.spacing.x;
        float cellHeight = height / rows - gridLayout.spacing.y;

        gridLayout.cellSize = new Vector2(cellWidth, cellHeight);
    }

    void GenerateGrid()
    {
        for (int i = 0; i < rows * columns; i++)
        {
            GameObject cell = Instantiate(cellPrefab, transform);
            cell.name = $"Cell_{i / columns}_{i % columns}";

            InputField inputField = cell.GetComponent<InputField>();
            if (inputField != null)
                inputField.text = "";  // Resetear el texto al instanciar
        }
    }
}