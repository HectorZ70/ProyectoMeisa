using UnityEngine;

public class LinksSpawner : MonoBehaviour
{
    public GameObject linkPrefab;
    public Transform parentCanvas;
    
    public void ShowLinks(RectTransform nodeTrans)
    {
        GameObject links = Instantiate(linkPrefab, parentCanvas);

        RectTransform linkRect = links.GetComponent<RectTransform>();
        Vector2 nodePos = nodeTrans.anchoredPosition;

        linkRect.anchoredPosition = nodePos + new Vector2(0, 30);
    }
}
