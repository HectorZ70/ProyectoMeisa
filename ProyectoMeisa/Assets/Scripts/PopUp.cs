using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;

public class PopUp : MonoBehaviour
{
    public TMP_InputField rowsInput;
    public TMP_InputField colsInput;
    public TMP_Dropdown dropdown;
    public TMP_InputField startDateInput;
    public TMP_InputField endDateInput;


    public ScrollableGrid dateGrid;
    public VirtualizedGrid virtualizedGrid;

    private GridManager gridManager;
    private PopUpSpawner popupSpawner;

    public void Init(GridManager manager, PopUpSpawner spawner, ScrollableGrid dateGrid, VirtualizedGrid virtualGrid)
    {
        gridManager = manager;
        popupSpawner = spawner;
        this.dateGrid = dateGrid;
        this.virtualizedGrid = virtualGrid;

        if (dropdown == null)
            dropdown = GetComponentInChildren<TMP_Dropdown>();


        if (dropdown != null)
        {
            dropdown.onValueChanged.AddListener(OnDropdownChanged);
        }
    }

    private Color GetSelectedColor()
    {
        return dropdown.value switch
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

   public void OnAccept()
{
    if (DateTime.TryParse(startDateInput.text, out DateTime startDate) &&
        DateTime.TryParse(endDateInput.text, out DateTime endDate))
    {
        Dictionary<int, DateTime> visibleDates = dateGrid.GetVisibleDateColumns();
        Color selectedColor = GetSelectedColor();
        virtualizedGrid.HighlightColumnsByDateRange(startDate, endDate, visibleDates, selectedColor);
        Destroy(gameObject);
    }
    else
    {
        Debug.LogWarning("Fechas inválidas.");
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