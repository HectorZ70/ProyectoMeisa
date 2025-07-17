using UnityEngine;
using UnityEngine.EventSystems;

public class ArrowSpawner : MonoBehaviour
{
    public GameObject arrowPrefab;
    public Transform canvasTrans;

    private RectTransform origin;

    public void SetOrigin(RectTransform originTrans)
    {
        origin = originTrans;
    }

    void Update()
    {
        if(origin != null && Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;

            Vector2 mousePos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasTrans as RectTransform,
                Input.mousePosition,
                null,
                out mousePos
                );

            GameObject arrow = Instantiate(arrowPrefab, canvasTrans);
            RectTransform arrowRect = arrow.GetComponent<RectTransform>();

            Vector2 start = origin.anchoredPosition;
            Vector2 end = mousePos;
            Vector2 direction = end - start;

            arrowRect.anchoredPosition = start + direction / 2f;
            arrowRect.sizeDelta = new Vector2(direction.magnitude, 5f);
            arrowRect.rotation = Quaternion.FromToRotation(Vector3.right, direction);

            origin = null;
        }
    }
}
