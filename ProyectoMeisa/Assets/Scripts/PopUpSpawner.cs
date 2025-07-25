using TMPro;
using UnityEngine;

public class PopUpSpawner : MonoBehaviour
{
    public GameObject[] popupPrefabs;
    public Transform parentCanvas;
    public GridManager gridManager;

    public VirtualizedGrid virtualizedGrid;
    public VirtualizedGrid virtualizedGrid2;
    public ScrollableGrid dateGrid;

    public PopUp popupInstance;

    void Start()
    {
        virtualizedGrid.popup = popupInstance;   
        virtualizedGrid2.popup = popupInstance;

        virtualizedGrid.linkedGrid = virtualizedGrid2;
        virtualizedGrid2.linkedGrid = virtualizedGrid;
    }
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
            popupScript.Init(gridManager, this, virtualizedGrid, dateGrid, virtualizedGrid2);
            virtualizedGrid.linkedGrid = virtualizedGrid2;
            virtualizedGrid2.linkedGrid = virtualizedGrid;
        }
    }
}