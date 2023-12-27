using System;
using UnityEngine;
using UnityEngine.UI;

public class DropdownHandler : MonoBehaviour
{
    private Dropdown _dropdown;
    
    private Details _details;
    [SerializeField] private int dropdownIndex;

    [SerializeField] private Text optionNameText;
    
    private void Awake()
    {
        _dropdown = GetComponent<Dropdown>();
    }

    private void Start()
    {
        _dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
    }
    
    public void Init(Details details, int dropdownIndex)
    {
        _details = details;
        this.dropdownIndex = dropdownIndex;
        
        optionNameText.text = _details.entrySO.optionList[dropdownIndex].optionName;
        _dropdown.ClearOptions();
        _dropdown.AddOptions(_details.entrySO.optionList[dropdownIndex].availableOptions);
    }

    private void OnDropdownValueChanged(int selectedIndex)
    {
        _details.OnDropdownValueChanged(dropdownIndex, selectedIndex);
    }
}