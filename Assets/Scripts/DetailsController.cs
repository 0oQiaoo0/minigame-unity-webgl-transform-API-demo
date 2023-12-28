using System;
using UnityEngine;
using UnityEngine.UI;

public class DetailsController : MonoBehaviour
{
    [Header("Entry Data")]
    [SerializeField] private EntrySO entrySO;
    
    [Header("Elements")]
    [SerializeField] private GameObject optionPrefab;
    [SerializeField] private Transform optionsTransform;
    
    [Header("Text")]
    [SerializeField] private Text titleText;
    [SerializeField] private Text APIText;
    [SerializeField] private Text descriptionText;
    [SerializeField] private Text startButtonText;
    
    [Header("Button")]
    [SerializeField] private Button startButton;

    [Header("Title Transform")] 
    [SerializeField] private RectTransform title;
    [SerializeField] private RectTransform backButton;
    
    private Details _details;

    private void Start()
    {
        title.anchoredPosition = new Vector2(title.anchoredPosition.x,  -125f - (float)GameManager.Instance.systemInfo.safeArea.top);
        backButton.anchoredPosition = new Vector2(backButton.anchoredPosition.x, -125f - (float)GameManager.Instance.systemInfo.safeArea.top);
    }

    private void ClearDetails()
    {
        Destroy(_details);
        
        var childCount = optionsTransform.childCount;
        
        for (var i = childCount - 1; i >= 0; i--)
        {
            var child = optionsTransform.GetChild(i);
            Destroy(child.gameObject);
        }
        
        startButton.onClick.RemoveAllListeners();
    }
    
    public void Init(EntrySO so)
    {
        ClearDetails();
        
        entrySO = so;

        titleText.text = so.entryName;
        APIText.text = so.entryAPI;
        descriptionText.text = so.entryDescription;

        _details = (Details)gameObject.AddComponent(entrySO.EntryScriptType);
        _details.Init(entrySO);
        
        for(var i = 0; i < entrySO.optionList.Count; i++)
        {
            var optionObj = Instantiate(optionPrefab, optionsTransform);
            optionObj.name = entrySO.optionList[i].optionName;
            optionObj.GetComponentInChildren<DropdownHandler>().Init(_details, i);
        }
        
        startButtonText.text = entrySO.buttonText;
        startButton.onClick.AddListener(() =>
        {
            _details.Run();
        });
    }
}
