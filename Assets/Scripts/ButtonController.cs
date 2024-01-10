using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    private Button _button;

    private void Awake()
    {
        // 获取子对象的 Button 组件
        _button = transform.Find("Button").GetComponent<Button>();
    }
    
    // 添加按钮监听事件
    public void AddButtonListener(UnityAction action)
    {
        _button.onClick.AddListener(action);
    }
}