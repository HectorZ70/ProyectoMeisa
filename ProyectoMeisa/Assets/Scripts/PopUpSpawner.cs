using TMPro;
using UnityEngine;

public class PopUpSpawner : MonoBehaviour
{
    public GameObject[] popupPrefabs;
    public Transform parentCanvas;
    public GridManager gridManager;

    public ScrollableGrid dateGrid;
    public VirtualizedGrid virtualizedGrid;
    public VirtualizedGrid virtualizedGrid2;

    public void ShowPopup(int index)
    {
        if (index < 0 || index >= popupPrefabs.Length)
        {
            Debug.LogWarning("indice de popup fuera de rango.");
            return;
        }

        GameObject popup = Instantiate(popupPrefabs[index], parentCanvas);

        PopUp popupScript = popup.GetComponentInChildren<PopUp>();
        if (popupScript != null)
        {
            popupScript.Init(gridManager, this, dateGrid, virtualizedGrid, virtualizedGrid2);
        }
    }
}