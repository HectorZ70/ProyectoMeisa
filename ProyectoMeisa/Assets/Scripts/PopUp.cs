using UnityEngine;
using TMPro;

public class PopUp : MonoBehaviour
{
    public TMP_InputField rowsInput;
    public TMP_InputField colsInput;

    private GridManager gridManager;

    public void Init(GridManager manager)
    {
        gridManager = manager;
    }

    public void OnAccept()
    {
        if (int.TryParse(rowsInput.text, out int rows) &&
            int.TryParse(colsInput.text, out int cols))
        {
            gridManager.ResizeGrid(rows, cols);
            Destroy(gameObject);
        }
        else
        {
            Debug.LogWarning("Entradas inválidas.");
        }
    }

    public void Close()
    {
        Destroy(gameObject);
    }
}