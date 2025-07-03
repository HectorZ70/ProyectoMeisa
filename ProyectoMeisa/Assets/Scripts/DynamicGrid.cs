using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DynamicGrid : MonoBehaviour
{
    public RectTransform content;        // Referencia al Content del Scroll View
    public GameObject inputFieldPrefab;  // Prefab del InputField
    public int rows = 10;
    public int columns = 5;

    void Start()
    {
        PopulateGrid(rows, columns);
    }

    void PopulateGrid(int rows, int columns)
    {
        for (int i = 0; i < rows * columns; i++)
        {
            GameObject field = Instantiate(inputFieldPrefab, content);
            field.name = $"InputField_{i / columns}_{i % columns}";
            field.GetComponentInChildren<TMP_InputField>().text = ""; // si usas TMP
        }

        // Ajusta el tamaño del Content para scroll automático si no lo hace el ContentSizeFitter
        float cellHeight = 30f; // o el tamaño real del prefab
        float spacingY = 10f;   // el valor de spacing del Grid Layout
        float totalHeight = (cellHeight + spacingY) * rows;

        content.sizeDelta = new Vector2(content.sizeDelta.x, totalHeight);
    }
}