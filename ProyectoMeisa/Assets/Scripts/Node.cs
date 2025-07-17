using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour ,IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerClickHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    public GameObject links;
    public LinksSpawner linksSpawner;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        linksSpawner = GetComponent<LinksSpawner>();
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (canvas == null) return;
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnBeginDrag(PointerEventData eventData){}

    public void OnEndDrag(PointerEventData eventData){}

    public void OnPointerClick(PointerEventData eventData)
    {
        linksSpawner.ShowLinks(this.transform as RectTransform);
    }
}
