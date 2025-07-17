using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    public GameObject cellPrefab;  
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

        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        AdjustCellSize();

        GenerateGrid();
    }

    void AdjustCellSize()
    {
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
                inputField.text = "";  
        }
    }
}