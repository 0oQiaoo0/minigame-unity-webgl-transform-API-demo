using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Category : MonoBehaviour
{
    private RectTransform _contentRectTransform;
    
    [SerializeField] private GameObject entryBlocks;
    [SerializeField] private Text categoryText;
    [SerializeField] private Image categoryImage;
    
    public Sprite image;
    
    public float unfoldAlpha = 0.5f;

    private bool _isUnfold = false;

    private void Awake()
    {
        _contentRectTransform = transform.parent.GetComponent<RectTransform>();
    }

    private void Start()
    {
        categoryText.text = gameObject.name;
        categoryImage.sprite = image;
    }

    private static Color SetColorWithAlpha(Color color, float alpha)
    {
        return new Color(color.r, color.g, color.b, alpha);
    }
    
    public void OnClick()
    {
        _isUnfold = !_isUnfold;
        
        categoryText.color = SetColorWithAlpha(categoryText.color, _isUnfold ? unfoldAlpha : 1f);
        categoryImage.color = SetColorWithAlpha(categoryImage.color, _isUnfold ? unfoldAlpha : 1f);

        entryBlocks.SetActive(_isUnfold);
        
        LayoutRebuilder.ForceRebuildLayoutImmediate(_contentRectTransform);
    }
}
