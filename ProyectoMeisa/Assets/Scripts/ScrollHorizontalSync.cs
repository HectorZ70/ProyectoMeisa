using UnityEngine;
using UnityEngine.UI;

public class ScrollHorizontalSync : MonoBehaviour
{
    public ScrollRect source;
    public ScrollRect target;

    bool syncing = false;

    void Start()
    {
        if(source != null && target != null)
        {
            source.onValueChanged.AddListener(OnSourceScroll);
            target.onValueChanged.AddListener(OnTargetScroll);
        }
    }

    void OnSourceScroll(Vector2 pos)
    {
        if (syncing) return;
        syncing = true;
        target.horizontalNormalizedPosition = source.horizontalNormalizedPosition;
        syncing = false;
    }

    void OnTargetScroll(Vector2 pos)
    {
        if (syncing) return;
        syncing = true;
        source.horizontalNormalizedPosition = target.horizontalNormalizedPosition;
        syncing = false;
    }
}
