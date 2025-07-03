using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class Cells : MonoBehaviour
{
    public RectTransform content;              // El objeto "Content"
    public GameObject inputFieldPrefab;        // El prefab del InputField
    public int rows = 10;                      // Número de filas
    public int columns = 5;                    // Número de columnas

    void Start()
    {
        SpawnGrid(rows, columns);
    }

    void SpawnGrid(int rows, int columns)
    {
        int totalCells = rows * columns;

        for (int i = 0; i < totalCells; i++)
        {
            GameObject cell = Instantiate(inputFieldPrefab, content);
            cell.name = $"Cell_{i / columns}_{i % columns}";

            // Opcional: establecer texto por defecto
            var input = cell.GetComponent<TMP_InputField>(); // O InputField si no usas TMP
            if (input != null)
                input.text = "";
        }
    }
}