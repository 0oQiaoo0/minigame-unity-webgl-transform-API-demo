using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Category : MonoBehaviour
{
    public Text categoryText;
    public Image categoryImage;
    
    public Sprite image;
    
    public float unfoldAlpha = 0.5f;

    private bool isUnfold = false;

    private void Start()
    {
        categoryText.text = gameObject.name;
        categoryImage.sprite = image;
    }

    private Color SetColorWithAlpha(Color color, float alpha)
    {
        return new Color(color.r, color.g, color.b, alpha);
    }
    
    private void Unfold()
    {
        categoryText.color = SetColorWithAlpha(categoryText.color, unfoldAlpha);
        categoryImage.color = SetColorWithAlpha(categoryImage.color, unfoldAlpha);
    }
    
    private void Fold()
    {
        categoryText.color = SetColorWithAlpha(categoryText.color, 1f);
        categoryImage.color = SetColorWithAlpha(categoryImage.color, 1f);
    }
    
    public void OnClick()
    {
        if (isUnfold)
        {
            Fold();
        }
        else
        {
            Unfold();
        }

        isUnfold = !isUnfold;
    }
}
