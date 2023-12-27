using System;
using UnityEngine;
using UnityEngine.UI;

public class DropdownHandler : MonoBehaviour
{
    private Dropdown _dropdown;
    
    [SerializeField] private Details details;
    [SerializeField] private int dropdownIndex;

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
        this.details = details;
        this.dropdownIndex = dropdownIndex;
    }

    private void OnDropdownValueChanged(int selectedIndex)
    {
        details.OnDropdownValueChanged(dropdownIndex, selectedIndex);
    }
}