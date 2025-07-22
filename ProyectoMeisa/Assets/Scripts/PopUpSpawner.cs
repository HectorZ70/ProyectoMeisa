using TMPro;
using UnityEngine;

public class PopUpSpawner : MonoBehaviour
{
    public GameObject[] popupPrefabs;
    public Transform parentCanvas;
    public GridManager gridManager;

    public VirtualizedGrid virtualizedGrid;
    public ScrollableGrid dateGrid;

    public void ShowPopup(int index)
    {
        if (index < 0 || index >= popupPrefabs.Length)
        {
            return;
        }

        GameObject popup = Instantiate(popupPrefabs[index], parentCanvas);

        PopUp popupScript = popup.GetComponentInChildren<PopUp>();
        if (popupScript != null)
        {
            popupScript.Init(gridManager, this, virtualizedGrid, dateGrid);
        }
    }
}