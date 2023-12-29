using UnityEngine;

public abstract class Details : MonoBehaviour
{
    public EntrySO entrySO;

    public string[] options;

    public void Init(EntrySO so)
    {
        entrySO = so;
        options = new string[entrySO.optionList.Count];
    }

    public void OnDropdownValueChanged(int dropdownIndex, int optionIndex)
    {
        var selectedOption = entrySO.optionList[dropdownIndex].availableOptions[optionIndex];
        
        options[dropdownIndex] = selectedOption;
    }
    
    public void Run()
    {
        TestAPI(options);
    }

    protected abstract void TestAPI(string[] args);
}
