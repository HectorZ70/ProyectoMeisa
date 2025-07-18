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

                Debug.Log("Se ha creado la flecha");

                origin = null;
            }
        }
    }
}
