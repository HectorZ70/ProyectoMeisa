using UnityEngine;
using UnityEngine.EventSystems;

public class Link : MonoBehaviour, IPointerClickHandler
{
    public ArrowSpawner arrowSpawner;
    public void OnPointerClick(PointerEventData eventData)
    {
        RectTransform thisPoint = this.transform as RectTransform;

        if (arrowSpawner.HasOrigin())
        {
            arrowSpawner.SetTarget(thisPoint);
        }
        else 
        {
            arrowSpawner.SetOrigin(thisPoint);
        }
        eventData.Use();
    }
}
