 using System;
using UnityEngine;
using UnityEngine.UI;

public class Entry : MonoBehaviour
{
    [SerializeField] private EntrySO entrySO;

    [SerializeField] private Text entryNameText;
    
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void Start()
    {
        _button.onClick.AddListener(OnClick);
    }

    public void Init(EntrySO so)
    {
        entrySO = so;
        entryNameText.text = entrySO.entryName;
        gameObject.name = entrySO.entryName;
    }

    private void OnClick()
    {
        GameManager.Instance.SwitchCanvas();
        GameManager.Instance.detailsController.Init(entrySO);
    }
}