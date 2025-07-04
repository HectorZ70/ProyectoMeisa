using TMPro;
using UnityEngine;

public class PopUp : MonoBehaviour
{
    public TMP_InputField rowsInput;
    public TMP_InputField colsInput;
    public TMP_Dropdown dropdown; 

    private GridManager gridManager;
    private PopUpSpawner popupSpawner;

    public void Init(GridManager manager, PopUpSpawner spawner)
    {
        gridManager = manager;
        popupSpawner = spawner;

        if (dropdown == null)
            dropdown = GetComponentInChildren<TMP_Dropdown>();


        if (dropdown != null)
        {
            dropdown.onValueChanged.AddListener(OnDropdownChanged);
        }
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

    public void OnDropdownChanged(int index)
    {
        if (index > 0 && popupSpawner != null)
        {
            popupSpawner.ShowPopup(index - 1); 
            Destroy(gameObject); 
        }

        dropdown.value = 0; 
    }

    public void Close()
    {
        Destroy(gameObject);
    }
}