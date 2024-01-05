using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Category : MonoBehaviour
{
    private RectTransform _contentRectTransform;

    [Header("Category Data")] 
    [SerializeField] private CategorySO categorySO;

    [Header("References")]
    [SerializeField] private Text categoryText;
    [SerializeField] private Image categoryImage;

    [Header("Entries")]
    [SerializeField] private GameObject entryPrefab;
    [SerializeField] private GameObject entries;
    
    [Header("Expand")]
    [SerializeField] private float unfoldAlpha = 0.5f;
    private bool _isExpanded = false;

    private void Awake()
    {
        _contentRectTransform = transform.parent.GetComponent<RectTransform>();
    }

    public void Init(CategorySO so)
    {
        categorySO = so;
        
        gameObject.name = categorySO.categoryName;
        categoryText.text = categorySO.categoryName;
        categoryImage.sprite = categorySO.categorySprite;
        
        foreach (var entry in categorySO.entryList)
        {
            var entryObj = Instantiate(entryPrefab, entries.transform);
            entryObj.GetComponent<Entry>().Init(entry);
        }
    }

    private static Color SetColorWithAlpha(Color color, float alpha)
    {
        return new Color(color.r, color.g, color.b, alpha);
    }
    
    public void OnClick()
    {
        _isExpanded = !_isExpanded;
        
        categoryText.color = SetColorWithAlpha(categoryText.color, _isExpanded ? unfoldAlpha : 1f);
        categoryImage.color = SetColorWithAlpha(categoryImage.color, _isExpanded ? unfoldAlpha : 1f);

        entries.SetActive(_isExpanded);
        
        LayoutRebuilder.ForceRebuildLayoutImmediate(_contentRectTransform);
    }
}
