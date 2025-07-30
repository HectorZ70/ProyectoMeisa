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
    public RectTransform potentialTarget;

    private List<ArrowLink> arrows = new List<ArrowLink>();

    [System.Serializable]
    public class ArrowLink
    {
        public RectTransform origin;
        public RectTransform target;
        public RectTransform arrowRect;

      

        public void UpdatePosition(RectTransform canvasRect)
        {
            if (origin == null || target == null || arrowRect == null) return;

            Vector2 localOrigin = WorldToLocal(canvasRect, origin.position);
            Vector2 localTarget = WorldToLocal(canvasRect, target.position);

            Vector2 direction = localTarget - localOrigin;

            arrowRect.anchoredPosition = localOrigin + direction / 2f;
            arrowRect.sizeDelta = new Vector2(direction.magnitude, arrowRect.sizeDelta.y);
            arrowRect.rotation = Quaternion.FromToRotation(Vector3.right, direction);

            Debug.DrawLine(origin.position, target.position, Color.red);
        }

        private Vector3 WorldToLocal(RectTransform canvasRect, Vector3 worldPos)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, worldPos, null, out Vector2 localPoint);
            return localPoint;
        }
    }

    void Update()
    {
        for (int i = arrows.Count - 1; i >= 0; i--)
        {
            var arrow = arrows[i];
            if (arrow.origin == null || arrow.target == null)
            {
                if (arrow.arrowRect != null)
                    Destroy(arrow.arrowRect.gameObject);

                arrows.RemoveAt(i);
                continue;
            }

            arrow.UpdatePosition(canvasTrans as RectTransform);
        }
    }

    private void CreateArrow()
    {
        if (origin == null || potentialTarget == null) return;

        GameObject arrow = Instantiate(arrowPrefab, canvasTrans);
        RectTransform arrowRect = arrow.GetComponent<RectTransform>();

        ArrowLink arrowLink = new ArrowLink
        {
            origin = origin,
            target = potentialTarget,
            arrowRect = arrowRect
        };

        arrows.Add(arrowLink);

        origin = null;
        potentialTarget = null;
    }

    public void RemoveArrowsConnectedTo(Transform node)
    {
        List<ArrowLink> toRemove = new List<ArrowLink>();

        foreach (var arrow in arrows)
        {
            if ((arrow.origin != null && (arrow.origin == node || arrow.origin.IsChildOf(node))) ||
                (arrow.target != null && (arrow.target == node || arrow.target.IsChildOf(node))))
            {
                Destroy(arrow.arrowRect.gameObject);
                toRemove.Add(arrow);
            }
        }

        foreach (var item in toRemove)
        {
            arrows.Remove(item);
        }
    }

    public void SetOrigin(RectTransform originTrans)
    {
        origin = originTrans;
    }

    public void SetTarget(RectTransform targetTrans)
    {
        potentialTarget = targetTrans;
        CreateArrow();
    }

    public bool HasOrigin()
    {
        return origin != null;
    }
}