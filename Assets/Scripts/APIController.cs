using System;
using UnityEngine;

public class APIController : MonoBehaviour
{
    [Header("API Data")]
    [SerializeField] private APISO apiSO;
    
    [Header("Elements")]
    [SerializeField] private GameObject categoryPrefab;
    [SerializeField] private Transform categoriesTransform;
    
    [Header("Title Transform")]
    [SerializeField] private RectTransform title;
    
    private void Start()
    {
        title.anchoredPosition = new Vector2(title.anchoredPosition.x,  -125f - (float)GameManager.Instance.systemInfo.safeArea.top);
    }

    private void ClearCategories()
    {
        int childCount = categoriesTransform.childCount;

        for (int i = childCount - 1; i >= 0; i--)
        {
            Transform child = categoriesTransform.GetChild(i);
            DestroyImmediate(child.gameObject);
        }
    }
    
    public void Init()
    {
        ClearCategories();
        
        foreach (var category in apiSO.categoryList)
        {
            var categoryObj = Instantiate(categoryPrefab, categoriesTransform);
            categoryObj.GetComponent<Category>().Init(category);
        }
    }
}