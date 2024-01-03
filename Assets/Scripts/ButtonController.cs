using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }
    
    public void AddButtonListener(UnityAction action)
    {
        _button.onClick.AddListener(action);
    }
}
