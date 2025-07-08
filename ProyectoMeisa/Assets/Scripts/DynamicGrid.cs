using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DynamicGrid : MonoBehaviour
{
    public RectTransform content;        
    public GameObject inputFieldPrefab;  
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
            field.GetComponentInChildren<TMP_InputField>().text = ""; 
        }

        
        float cellHeight = 30f; 
        float spacingY = 10f;   
        float totalHeight = (cellHeight + spacingY) * rows;

        content.sizeDelta = new Vector2(content.sizeDelta.x, totalHeight);
    }
}