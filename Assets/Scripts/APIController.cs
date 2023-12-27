using UnityEngine;

public class APIController : MonoBehaviour
{
    [SerializeField] private APISO apiSO;
    
    [SerializeField] private GameObject categoryPrefab;
    [SerializeField] private Transform categoriesTransform;

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