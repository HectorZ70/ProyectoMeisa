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

    private int selectedRow = 0;

    public ScrollableGrid dateGrid;
    public VirtualizedGrid virtualizedGrid;
    public VirtualizedGrid virtualizedGrid2;
    public GameObject popupPanel;
    public FileIO fileIO;

    private GridManager gridManager;
    private PopUpSpawner popupSpawner;
    private Color selectedColor = Color.red;

    public void Init(GridManager manager, PopUpSpawner spawner, ScrollableGrid dateGrid, VirtualizedGrid virtualGrid, VirtualizedGrid virtualGrid2)
    {
        gridManager = manager;
        popupSpawner = spawner;
        this.dateGrid = dateGrid;
        this.virtualizedGrid = virtualGrid;
        this.virtualizedGrid2 = virtualGrid2;

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
        if (!DateTime.TryParse(startDateInput.text, out DateTime startDate) ||
         !DateTime.TryParse(endDateInput.text, out DateTime endDate))
        {
            return;
        }

        Dictionary<int, DateTime> visibleDates = dateGrid.GetVisibleDateColumns();
        List<int> highlightedCols = new();

        foreach (var kvp in visibleDates)
        {
            if (kvp.Value >= startDate && kvp.Value <= endDate)
                highlightedCols.Add(kvp.Key);
        }

        if (virtualizedGrid.selectedRow >= 0)
        {
            virtualizedGrid.HighlightColumnsByDateRange(
                startDate,
                endDate,
                visibleDates,
                virtualizedGrid.selectedRow,
                selectedColor
            );
        }

        if (highlightedCols.Count > 0 && customTextInput != null)
        {
            int firstCol = highlightedCols[0];
            string textToWrite = customTextInput.text;
            virtualizedGrid.WriteToCell(virtualizedGrid.selectedRow, firstCol, textToWrite);
        }
        Destroy(gameObject);
    }

    public void OnSortDropdownChanged(int index)
    {
        if (index == 0) return;

        int col = index - 1;


        string firstValue = virtualizedGrid2.ReadFromCell(0, col);
        bool isNumeric = int.TryParse(firstValue, out _);
        virtualizedGrid2.SortGridByColumn(col, isNumeric);

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

        switch (selectedOption)
        {
            case "Abrir":
                if (fileIO != null)
                {
                    fileIO.OpenFile();
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
            Dictionary<int, DateTime> visibleDates = dateGrid.GetVisibleDateColumns();
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


    public void Close()
    {
        Destroy(gameObject);
    }
}
