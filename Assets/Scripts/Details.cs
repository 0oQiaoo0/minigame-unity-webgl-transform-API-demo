using UnityEngine;

public abstract class Details : MonoBehaviour
{
    public EntrySO entrySO;

    protected string[] options;

    public void Init(EntrySO so)
    {
        entrySO = so;
        options = new string[entrySO.optionList.Count];
        for (var i = 0; i < options.Length; i++)
        {
            options[i] = entrySO.optionList[i].availableOptions[0];
        }
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
