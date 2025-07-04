using UnityEngine;
using TMPro;

public class DropdownHandler : MonoBehaviour
{
    public TMP_Dropdown dropdown;            
    public PopUpSpawner popupSpawner;       

    void Start()
    {
        dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
    }

    void OnDropdownValueChanged(int index)
    {
        if (index > 0)
        {
            popupSpawner.ShowPopup(index - 1); 
        }
        dropdown.value = 0;
    }

    private void OnDestroy()
    {
        dropdown.onValueChanged.RemoveListener(OnDropdownValueChanged);
    }
}