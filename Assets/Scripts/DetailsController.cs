using System.Collections.Generic;
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
    
    [Header("Result")]
    [SerializeField] private Text resultTitleText;
    [SerializeField] private GameObject resultPrefab;
    [SerializeField] private Transform resultsTransform;
    [HideInInspector] public List<GameObject> resultObjects;

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
        
        for (var i = optionsTransform.childCount - 1; i >= 0; i--)
        {
            var child = optionsTransform.GetChild(i);
            Destroy(child.gameObject);
        }
        
        startButton.onClick.RemoveAllListeners();

        resultObjects = new List<GameObject>();
        for(var i = resultsTransform.childCount - 1; i >= 0; i--)
        {
            var child = resultsTransform.GetChild(i);
            Destroy(child.gameObject);
        }
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
            optionObj.GetComponentInChildren<OptionDropdownHandler>().Init(_details, i);
        }
        
        ChangeButtonText(entrySO.initialButtonText);
        startButton.onClick.AddListener(() =>
        {
            _details.Run();
        });
        
        resultTitleText.text = entrySO.entryResultTitle;
        foreach (var result in entrySO.initialResultList)
        {
            AddResult(result);
        }
    }
    
    public void ChangeButtonText(string text)
    {
        startButtonText.text = text;
    }
    
    public int AddResult(Result result)
    {
        var resultObj = Instantiate(resultPrefab, resultsTransform);
        resultObj.GetComponent<ResultController>().ChangeContent(result.initialContent);
        resultObjects.Add(resultObj);
        if (result.isDisableInitially)
        {
            resultObj.SetActive(false);
        }
        return resultObjects.Count - 1;
    }
    
    public void RemoveResult(int index)
    {
        Destroy(resultObjects[index]);
        resultObjects.RemoveAt(index);
    }
    
    public void EnableResult(int index)
    {
        resultObjects[index].SetActive(true);
    }
    
    public void DisableResult(int index)
    {
        resultObjects[index].SetActive(false);
    }
    
    public void ChangeResultContent(int index, string content)
    {
        resultObjects[index].GetComponent<ResultController>().ChangeContent(content);
    }
}
