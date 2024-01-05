using System;
using UnityEngine;
using UnityEngine.Serialization;

public class APIController : MonoBehaviour
{
    [Header("API Data")]
    [SerializeField] private APISO apiSO;
    
    [Header("Elements")]
    [SerializeField] private GameObject categoryPrefab;
    [SerializeField] private Transform apiCategoriesTransform;
    [SerializeField] private GameObject abilityPrefab;
    [SerializeField] private Transform abilitiesTransform;
    
    [Header("Title Transform")]
    [SerializeField] private RectTransform title;
    
    private void Start()
    {
        title.anchoredPosition = new Vector2(title.anchoredPosition.x,  -125f - (float)GameManager.Instance.systemInfo.safeArea.top);
    }

    private void ClearCategories()
    {
        var childCount = apiCategoriesTransform.childCount;

        for (var i = childCount - 1; i >= 0; i--)
        {
            var child = apiCategoriesTransform.GetChild(i);
            DestroyImmediate(child.gameObject);
        }
    }
    
    private void ClearAbilities()
    {
        var childCount = abilitiesTransform.childCount;

        for (var i = childCount - 1; i >= 0; i--)
        {
            var child = abilitiesTransform.GetChild(i);
            DestroyImmediate(child.gameObject);
        }
    }
    
    public void Init()
    {
        ClearCategories();
        ClearAbilities();
        
        foreach (var category in apiSO.categoryList)
        {
            var categoryObj = Instantiate(categoryPrefab, apiCategoriesTransform);
            categoryObj.GetComponent<Category>().Init(category);
        }

        foreach (var ability in apiSO.abilityList)
        {
            var abilityObj = Instantiate(abilityPrefab, abilitiesTransform);
            abilityObj.GetComponent<Ability>().Init(ability);
        }
    }
}