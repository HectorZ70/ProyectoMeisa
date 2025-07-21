using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class NodeSpawner : MonoBehaviour
{
    public GameObject nodePrefab;
    public Transform parentCanvas;
    private Vector3 mousePosition;

    void Update()
    {
        ShowNode();
    }

    public void ShowNode()
    {
        if (Input.GetMouseButtonDown(1))
        {
            GameObject node = Instantiate(nodePrefab, parentCanvas);
            node.GetComponent<Node>().linksSpawner = this.GetComponent<LinkSpawner>();

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                parentCanvas as RectTransform,
                Input.mousePosition,
                null,
                out Vector2 localPos
            );

            node.GetComponent<RectTransform>().anchoredPosition = localPos;
        }

    }

}
