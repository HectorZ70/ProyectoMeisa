using UnityEngine;

public class LinksSpawner : MonoBehaviour
{
    public GameObject linkPrefab;
    public Transform parentCanvas;
    
    public GameObject ShowLinks(RectTransform nodeTrans)
    {
        GameObject links = Instantiate(linkPrefab, nodeTrans);
        RectTransform linkRect = links.GetComponent<RectTransform>();
        linkRect.anchorMin = new Vector2(0.5f, 0.5f);
        linkRect.anchorMax = new Vector2(0.5f, 0.5f);
        linkRect.pivot = new Vector2(0.5f, 0.5f);
        linkRect.localScale = Vector3.one;
        linkRect.localRotation = Quaternion.identity;
        linkRect.anchoredPosition = new Vector2(0f, 0f);

        return links;
    }
}
