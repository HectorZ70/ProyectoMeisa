using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ArrowSpawner : MonoBehaviour
{
    public GameObject arrowPrefab;
    public Transform canvasTrans;
    public GraphicRaycaster raycaster;

    public bool clickWasOnLink = false;

    private RectTransform origin;

    public void SetOrigin(RectTransform originTrans)
    {
        origin = originTrans;
    }

    void Update()
    {
        if (origin != null && Input.GetMouseButtonDown(0))
        {
            if (clickWasOnLink)
            {
                clickWasOnLink = false;

                Vector2 localMousePos;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    canvasTrans as RectTransform,
                    Input.mousePosition,
                    null,
                    out localMousePos
                );

                Vector2 localOriginPos = WorldToLocalPosition(canvasTrans as RectTransform, origin.position);

                Vector2 start = localOriginPos;
                Vector2 end = localMousePos;
                Vector2 direction = end - start;

                GameObject arrow = Instantiate(arrowPrefab, canvasTrans);
                RectTransform arrowRect = arrow.GetComponent<RectTransform>();

                arrowRect.anchoredPosition = start + direction / 2f;
                arrowRect.sizeDelta = new Vector2(direction.magnitude, 5f);
                arrowRect.rotation = Quaternion.FromToRotation(Vector3.right, direction);

                origin = null;
            }
        }
    }
    private Vector2 WorldToLocalPosition(RectTransform canvasRect, Vector3 worldPos)
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            worldPos,
            null,
            out localPoint
        );
        return localPoint;
    }
}
