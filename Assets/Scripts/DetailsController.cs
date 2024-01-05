using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
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
    [SerializeField] private Button initialButton;
    [SerializeField] private GameObject buttonBlockPrefab;
    [SerializeField] private Transform buttonsTransform;
    [HideInInspector] public List<GameObject> extraButtonBlockObjects;
    
    [Header("Result")]
    [SerializeField] private GameObject resultPrefab;
    [SerializeField] private Transform resultsTransform;
    [HideInInspector] public List<GameObject> resultObjects;
    
    [Header("Title Transform")] 
    [SerializeField] private RectTransform titleTransform;
    [SerializeField] private RectTransform backButtonTransform;
    
    private Details _details;

    private void Start()
    {
        titleTransform.anchoredPosition = new Vector2(titleTransform.anchoredPosition.x,  -125f - (float)GameManager.Instance.systemInfo.safeArea.top);
        backButtonTransform.anchoredPosition = new Vector2(backButtonTransform.anchoredPosition.x, -125f - (float)GameManager.Instance.systemInfo.safeArea.top);
    }

    private void ClearDetails()
    {
        // destroy details
        Destroy(_details);
        
        // clear options
        for (var i = optionsTransform.childCount - 1; i >= 0; i--)
        {
            var child = optionsTransform.GetChild(i);
            Destroy(child.gameObject);
        }
        
        // clear buttons
        initialButton.onClick.RemoveAllListeners();
        foreach (var i in extraButtonBlockObjects)
        {
            Destroy(i);
        }
        extraButtonBlockObjects = new List<GameObject>();

        // clear results
        RemoveAllResult();
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
        
        // generate options
        for(var i = 0; i < entrySO.optionList.Count; i++)
        {
            var optionObj = Instantiate(optionPrefab, optionsTransform);
            optionObj.name = entrySO.optionList[i].optionName;
            optionObj.GetComponentInChildren<OptionDropdownHandler>().Init(_details, i);
        }
        
        // set the initial button
        ChangeInitialButtonText(entrySO.initialButtonText);
        initialButton.onClick.AddListener(() =>
        {
            _details.Run();
        });
        // generate extra buttons
        foreach (var button in entrySO.extraButtonList)
        {
            var extraButtonBlock = Instantiate(buttonBlockPrefab, buttonsTransform);
            extraButtonBlockObjects.Add(extraButtonBlock);
            extraButtonBlock.GetComponentInChildren<Text>().text = button.buttonText;
            // extraButton.onClick.AddListener(() =>
            // {
            //     button.buttonAction.Invoke();
            // });
        }
        
        // generate results
        foreach (var result in entrySO.initialResultList)
        {
            AddResult(result);
        }
    }
    
    public void ChangeInitialButtonText(string text)
    {
        startButtonText.text = text;
    }
    
    public void BindExtraButtonAction(int index, UnityAction action)
    {
        extraButtonBlockObjects[index].GetComponent<ButtonController>()
            .AddButtonListener(action);
    }
    
    public GameObject AddResult(ResultData resultData)
    {
        var resultObj = Instantiate(resultPrefab, resultsTransform);
        resultObjects.Add(resultObj);
        
        ChangeResultTitle(resultObjects.Count - 1, resultData.initialTitleText);
        ChangeResultContent(resultObjects.Count - 1, resultData.initialContentText);
        if (resultData.isDisableInitially)
        {
            resultObj.SetActive(false);
        }
        return resultObj;
    }
    
    public void RemoveAllResult()
    {
        foreach (var obj in resultObjects)
        {
            Destroy(obj);
        }

        resultObjects = new List<GameObject>();
    }

    public void KeepFirstNResults(int n)
    {
        for (var i = n; i < resultObjects.Count; i++)
        {
            Destroy(resultObjects[i]);
        }
        
        resultObjects.RemoveRange(n, resultObjects.Count - n);
    }
    
    public void SetResultActive(int index, bool isActive)
    {
        resultObjects[index].SetActive(isActive);
    }

    public void ChangeResultTitle(int index, string title)
    {
        resultObjects[index].GetComponent<ResultController>().ChangeTitle(title);
    }
    
    public void ChangeResultContent(int index, string content)
    {
        resultObjects[index].GetComponent<ResultController>().ChangeContent(content);
    }
}
