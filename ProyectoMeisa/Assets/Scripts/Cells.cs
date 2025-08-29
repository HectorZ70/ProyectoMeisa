using UnityEngine;
using TMPro; 

public class Cells : MonoBehaviour
{
    public RectTransform content;              
    public GameObject inputFieldPrefab;        
    public int rows = 10;                      
    public int columns = 5;                    

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

            var input = cell.GetComponent<TMP_InputField>(); 
            if (input != null)
                input.text = "";
        }
    }
}