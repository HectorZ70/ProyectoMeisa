using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject cellPrefab;
    public int rows = 10;
    public int columns = 10;

    void Start()
    {
        GenerateGrid();
    }

    public void ResizeGrid(int newRows, int newColumns)
    {
        rows = newRows;
        columns = newColumns;

        // Destruir celdas actuales
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // Volver a generar la cuadrícula
        GenerateGrid();
    }

    void GenerateGrid()
    {
        for (int i = 0; i < rows * columns; i++)
        {
            GameObject cell = Instantiate(cellPrefab, transform);
            cell.name = $"Cell_{i / columns}_{i % columns}";

            Cells excelCell = cell.GetComponent<Cells>();
            if (excelCell != null)
                excelCell.SetValue("");
        }
    }
}