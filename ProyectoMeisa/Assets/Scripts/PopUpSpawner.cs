using TMPro;
using UnityEngine;

public class PopUpSpawner : MonoBehaviour
{
    public GameObject[] popupPrefabs;
    public Transform parentCanvas;
    public GridManager gridManager;

    public ScrollableGrid scrollableGrid;
    public VirtualizedGrid virtualizedGrid;
    public VirtualizedGrid virtualizedGrid2;

    public void ShowPopup(int index)
    {
        if (index < 0 || index >= popupPrefabs.Length)
        {
            Debug.LogWarning("Índice de popup fuera de rango.");
            return;
        }

        GameObject popup = Instantiate(popupPrefabs[index], parentCanvas);

        PopUp popupScript = popup.GetComponentInChildren<PopUp>();
        if (popupScript != null)
        {
            popupScript.Init(gridManager, this, scrollableGrid, virtualizedGrid, virtualizedGrid2);
        }
    }
}