using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class Link : MonoBehaviour, IPointerClickHandler
{
    public ArrowSpawner arrowSpawner;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (arrowSpawner.HasOrigin())
        {
            arrowSpawner.SetTarget(this.transform as RectTransform);
        }
        else 
        {
            arrowSpawner.SetOrigin(this.transform as RectTransform);
        }
        eventData.Use();
    }
}
