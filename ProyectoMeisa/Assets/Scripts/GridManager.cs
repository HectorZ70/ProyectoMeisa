using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject cellPrefab;  // El prefab con InputField + ExcelCell script
    public int rows = 10;
    public int columns = 10;

    void Start()
    {
        for (int i = 0; i < rows * columns; i++)
        {
            GameObject cell = Instantiate(cellPrefab, transform);
            cell.name = $"Cell_{i / columns}_{i % columns}";

            // Puedes acceder a su script directamente si quieres inicializar cosas
            Cells excelCell = cell.GetComponent<Cells>();
            excelCell.SetValue(""); // O inicializar con un número, etc.
        }
    }
}