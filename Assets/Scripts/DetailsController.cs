using UnityEngine;
using UnityEngine.UI;

public class DetailsController : MonoBehaviour
{
    [SerializeField] private EntrySO entrySO;
    
    [SerializeField] private GameObject optionPrefab;
    [SerializeField] private Transform optionsTransform;
    
    [SerializeField] private Text titleText;
    [SerializeField] private Text APIText;
    [SerializeField] private Text descriptionText;
    [SerializeField] private Button startButton;
    private Text _startButtonText;
    
    private Details _details;

    private void Awake()
    {
        _startButtonText = startButton.GetComponentInChildren<Text>();
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
        
        _startButtonText.text = entrySO.buttonText;
        startButton.onClick.AddListener(() =>
        {
            _details.Run();
        });
    }
}
