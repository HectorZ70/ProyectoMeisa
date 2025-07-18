using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class Link : MonoBehaviour, IPointerClickHandler
{
    public ArrowSpawner arrowSpawner;
    public void OnPointerClick(PointerEventData eventData)
    {
        arrowSpawner.clickWasOnLink = true;
        Debug.Log("Se ha clicado al hijo");
        arrowSpawner.SetOrigin(this.transform as RectTransform);

        eventData.Use();
    }
}
