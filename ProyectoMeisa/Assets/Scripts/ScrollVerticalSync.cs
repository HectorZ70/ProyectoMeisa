using UnityEngine;
using UnityEngine.UI;

public class ScrollVerticalSync : MonoBehaviour
{
    public ScrollRect source;
    public ScrollRect target;

    bool syncing;

    void Start()
    {
        if (source != null && target != null)
        {
            source.onValueChanged.AddListener(OnScroll);
        }
    }

    void OnScroll(Vector2 scrollPos)
    {
        target.verticalNormalizedPosition = source.verticalNormalizedPosition;
    }
}
