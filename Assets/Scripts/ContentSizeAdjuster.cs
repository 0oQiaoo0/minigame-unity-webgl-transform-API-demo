using System;
using UnityEngine;
using UnityEngine.UI;

public class ContentSizeAdjuster : MonoBehaviour
{
    private RectTransform _content;
    public RectTransform verticalLayoutGroup;

    private void Awake()
    {
        _content = GetComponent<RectTransform>();
    }

    private void Start()
    {
        UpdateContentSize();
    }

    private void UpdateContentSize()
    {
        Debug.Log(verticalLayoutGroup.rect.height);
        _content.sizeDelta = new Vector2(_content.sizeDelta.x, verticalLayoutGroup.rect.height);
    }
}