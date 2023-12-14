using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Content : MonoBehaviour
{
    private RectTransform _rectTransform;
    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(_rectTransform);
    }
}
