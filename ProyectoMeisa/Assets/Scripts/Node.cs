using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerClickHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    public GameObject links;

    public LinkSpawner linksSpawner;
    private bool wasDrag = false;
    private bool linksExist = false;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (canvas == null) return;
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        wasDrag = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        wasDrag = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.pointerPress != this.gameObject) return;
        if (!wasDrag && !linksExist)
        {
            links = linksSpawner.ShowLinks(this.transform as RectTransform);
            linksExist = true;
        }

        else if (!wasDrag && linksExist)
        {
            Destroy(links);
            links = null;
            linksExist = false;
        }

        if (eventData.button == PointerEventData.InputButton.Middle)
        {
            Destroy(this.gameObject); 
        }
    }

    private void OnDestroy()
    {
        if(linksSpawner != null && linksSpawner.arrowSpawner != null)
        {
            linksSpawner.arrowSpawner.RemoveArrowsConnectedTo(this.transform);
        }
    }

}
