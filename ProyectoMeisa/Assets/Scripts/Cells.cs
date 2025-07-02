using UnityEngine;
using UnityEngine.UI;

public class Cells : MonoBehaviour
{
    private InputField input;

    void Awake()
    {
        input = GetComponent<InputField>();
    }

    public string GetValue()
    {
        return input.text;
    }

    public void SetValue(string val)
    {
        input.text = val;
    }

    public void OnValueChanged(string val)
    {
        Debug.Log("Celda cambió a: " + val);
    }
}