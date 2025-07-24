using UnityEngine;
using TMPro;

public class DropdownHandler : MonoBehaviour
{
    public TMP_Dropdown dropdown;            
    public PopUpSpawner popupSpawner;       

    void Start()
    {
        if (dropdown == null)
            dropdown = GetComponent<TMP_Dropdown>();

        if (dropdown != null)
            dropdown.onValueChanged.AddListener(OnDropdownValueChanged);

        if (popupSpawner == null)
            popupSpawner = FindFirstObjectByType<PopUpSpawner>();

    }

    void OnDropdownValueChanged(int index)
    {
        if (index > 0 && popupSpawner != null)
        {
            popupSpawner.ShowPopup(index); 
        }
        dropdown.value = 0;
    }

    private void OnDestroy()
    {
        if (dropdown != null)
            dropdown.onValueChanged.RemoveListener(OnDropdownValueChanged);
    }
}