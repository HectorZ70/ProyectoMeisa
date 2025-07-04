using TMPro;
using UnityEngine;

public class PopUpSpawner : MonoBehaviour
{
    public GameObject[] popupPrefabs;
    public Transform parentCanvas;
    public GridManager gridManager;

    public void ShowPopup(int index)
    {
        if (index < 0 || index >= popupPrefabs.Length)
        {
            Debug.LogWarning("�ndice de popup fuera de rango.");
            return;
        }

        GameObject popup = Instantiate(popupPrefabs[index], parentCanvas);

        // Inicializa l�gica interna del PopUp
        PopUp popupScript = popup.GetComponent<PopUp>();
        if (popupScript != null)
        {
            popupScript.Init(gridManager);
        }

        // Aqu� conectas el Dropdown interno del prefab con el PopUpSpawner
        TMP_Dropdown dropdown = popup.GetComponentInChildren<TMP_Dropdown>();
        if (dropdown != null)
        {
            dropdown.onValueChanged.AddListener((int selectedIndex) =>
            {
                Debug.Log("Dropdown del popup seleccion�: " + selectedIndex);
                // Ejemplo: si quieres abrir otro popup al elegir una opci�n dentro del popup
                if (selectedIndex > 0)
                {
                    ShowPopup(selectedIndex - 1);
                    dropdown.value = 0; // Reset para que puedas volver a elegir la misma opci�n
                }
            });
        }
    }
}