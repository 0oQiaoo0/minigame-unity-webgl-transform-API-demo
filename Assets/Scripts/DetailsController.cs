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
    
    private Details _details;
    
    private void ClearDetails()
    {
        Destroy(_details);
        
        int childCount = optionsTransform.childCount;
        
        for (int i = childCount - 1; i >= 0; i--)
        {
            Transform child = optionsTransform.GetChild(i);
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
            Debug.Log(entrySO.optionList[i].optionName);
            var optionObj = Instantiate(optionPrefab, optionsTransform);
            optionObj.name = entrySO.optionList[i].optionName;
            optionObj.GetComponentInChildren<DropdownHandler>().Init(_details, i);
        }
        
        startButton.onClick.AddListener(() =>
        {
            _details.Run();
        });
    }
}
