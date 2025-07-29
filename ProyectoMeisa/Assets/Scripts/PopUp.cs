using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PopUp : MonoBehaviour
{
    public TMP_InputField rowsInput;
    public TMP_InputField colsInput;
    public TMP_Dropdown popupDropdown;
    public TMP_Dropdown colorDropdown;
    public TMP_Dropdown filterDropdown;
    public TMP_Dropdown fileDropdown;
    public TMP_InputField startDateInput;
    public TMP_InputField endDateInput;
    public TMP_InputField customTextInput;
    public Button guardarBtn, cargarBtn;
    public TMP_Text numOpText;
    public TMP_Text nameOpText;
    public TMP_Text contractOpText;
    public TMP_Text posOpText;

    public VirtualizedGrid virtualizedGrid;
    public VirtualizedGrid virtualizedGrid2;
    public ScrollableGrid datesGrid;
    public GameObject popupPanel;
    public FileIO fileIO;

    private GridManager gridManager;
    private PopUpSpawner popupSpawner;
    private Color selectedColor = Color.red;
    public static PopUp CurrentInstance;

    private void Start()
    {
        //guardarBtn.onClick.AddListener(virtualizedGrid.Save);
        //cargarBtn.onClick.AddListener(virtualizedGrid.Load);
    }

    public void Init(GridManager manager, PopUpSpawner spawner, VirtualizedGrid virtualGrid, ScrollableGrid dateGrid, VirtualizedGrid virtualGrid2 = null)
    {
        gridManager = manager;
        popupSpawner = spawner;
        this.virtualizedGrid = virtualGrid;
        this.virtualizedGrid2 = virtualGrid2;
        this.datesGrid = dateGrid;
        CurrentInstance = this;
   

        if (popupDropdown == null)
            popupDropdown = transform.Find("PopupDropdown")?.GetComponent<TMP_Dropdown>();

        if (colorDropdown == null)
            colorDropdown = transform.Find("ColorDropdown")?.GetComponent<TMP_Dropdown>();

        if (popupDropdown != null)
            popupDropdown.onValueChanged.AddListener(OnPopupDropdownChanged);

        if (colorDropdown != null)
            colorDropdown.onValueChanged.AddListener(OnColorDropdownChanged);

        if (filterDropdown != null)
        {
            filterDropdown.onValueChanged.AddListener(OnSortDropdownChanged);
        }

        UpdateDetailsFromRow();
    }

    public void ConfirmExit()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;

#endif
    }
    public void CancelExit()
    {
        popupPanel.SetActive(false);
    }

    public void OnAccept()
    {
        int? selectedRow = virtualizedGrid.selectedRow ?? virtualizedGrid2?.selectedRow;

        if (selectedRow == null)
        {
            Debug.LogWarning("No se ha seleccionado ninguna fila.");
            return;
        }

        int row = selectedRow.Value;

        if (DateTime.TryParse(startDateInput.text, out DateTime startDate) &&
            DateTime.TryParse(endDateInput.text, out DateTime endDate))
        {
            Dictionary<int, DateTime> allDates = datesGrid.GetAllDateColumns();
            List<int> highlightedCols = new();

            foreach (var kvp in allDates)
            {
                if (kvp.Value >= startDate && kvp.Value <= endDate)
                    highlightedCols.Add(kvp.Key);
            }

            foreach (int col in highlightedCols)
                virtualizedGrid.HighlightSingleCell(row, col, selectedColor);

            if (virtualizedGrid2 != null)
            {
                virtualizedGrid2.selectedRow = selectedRow; 
                foreach (int col in highlightedCols)
                   virtualizedGrid2.HighlightSingleCell(row, col, selectedColor);
            }

            if (highlightedCols.Count > 0 && customTextInput != null)
            {
                int firstCol = highlightedCols[0];
                string textToWrite = customTextInput.text;
                virtualizedGrid.WriteToCell(row, firstCol, textToWrite);
            }

            Destroy(gameObject);
        }
        else
        {
            Debug.LogWarning("Fechas inválidas.");
        }
    }

    public void OnSortDropdownChanged(int index)
{
        if (index == 0) return; 

        int col = index - 1;

        
            string firstValue = virtualizedGrid.ReadFromCell(0, col);
            bool isNumeric = int.TryParse(firstValue, out _);
            virtualizedGrid.SortGridByColumn(col, isNumeric);
        
        filterDropdown.SetValueWithoutNotify(0);
        gameObject.SetActive(false);
    }

public void OpenFileAndLoadGrid() 
    {
        GridSaveData data = GridSaveLoad.Load();
        if (data != null)
        {
            GridLoadBuffer.DataToLoad = data;
            SceneManager.LoadScene("ListScene");
        }
    }

public void OnFileDropownChanged(int index)
    {
        string selectedOption = fileDropdown.options[index].text;

        switch(selectedOption)
        {
            case "Abrir":
                if(fileIO != null)
                {
                    fileIO.OpenFile();
                }
                else
                {
                    Debug.LogWarning("Referencia a FileIO no asignada");
                }
                    break;
        }
        fileDropdown.SetValueWithoutNotify(0);
    }

    public void OnPopupDropdownChanged(int index)
    {
        if (index > 0 && popupSpawner != null)
        {
            popupSpawner.ShowPopup(index - 1); 
            Destroy(gameObject); 
        }

        popupDropdown.value = 0; 
    }

    public void OnColorDropdownChanged(int index)
    {
        selectedColor = GetSelectedColor(index);
    }

    private Color GetSelectedColor(int index)
    {
        return index switch
        {
            0 => Color.red,
            1 => Color.yellow,
            2 => Color.grey,
            3 => Color.orange,
            4 => Color.blue,
            5 => Color.pink,
            6 => Color.black,
            7 => Color.white,
            8 => Color.lightGreen,
            _ => Color.violet
        };
    }

    public void ClearHighlightedCells()
    {
        if (DateTime.TryParse(startDateInput.text, out DateTime startDate) &&
            DateTime.TryParse(endDateInput.text, out DateTime endDate))
        {
            Dictionary<int, DateTime> visibleDates = datesGrid.GetAllDateColumns();
            List<int> columnsToClear = new();

            foreach (var kvp in visibleDates)
            {
                if (kvp.Value >= startDate && kvp.Value <= endDate)
                    columnsToClear.Add(kvp.Key);
            }

            if (columnsToClear.Count > 0)
            {
                virtualizedGrid.ClearCellsInColumns(columnsToClear);
            }
            else
            {
                Debug.Log("No se encontraron columnas en el rango de fechas.");
            }
        }
        else
        {
            Debug.LogWarning("Fechas inválidas.");
        }
    }

    public void UpdateDetailsFromRow()
    {
        Debug.Log("UpdateDetailsFromRow() se está ejecutando");

        if (virtualizedGrid2 == null || virtualizedGrid2.selectedRow == null)
        {
            Debug.LogWarning("No hay una fila seleccionada en VirtualizedGrid2.");
            return;
        }

        int rowIndex = virtualizedGrid2.selectedRow.Value;

        string numOp = virtualizedGrid2.ReadFromCell(rowIndex, 0);
        string op = virtualizedGrid2.ReadFromCell(rowIndex, 1);
        string contractOp = virtualizedGrid2.ReadFromCell(rowIndex, 2);
        string posOp = virtualizedGrid2.ReadFromCell(rowIndex, 3);

        // Asegurate de tener estas referencias seteadas en el inspector
        if (numOpText != null) numOpText.text = numOp;
        if (nameOpText != null) nameOpText.text = op;
        if (contractOpText != null) contractOpText.text = contractOp;
        if (posOpText != null) posOpText.text = posOp;
    }

    public void Close()
    {
        Destroy(gameObject);
    }
}