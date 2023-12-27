using UnityEngine;

public abstract class Details : MonoBehaviour
{
    public EntrySO entrySO;

    public string[] options;

    private void Start()
    {
        options = new string[entrySO.optionList.Count];
    }

    public void Init(EntrySO so)
    {
        entrySO = so;
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

    protected abstract void TestAPI(params string[] args);
}
