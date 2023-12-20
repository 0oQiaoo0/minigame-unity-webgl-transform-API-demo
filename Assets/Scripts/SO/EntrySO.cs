using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EntrySO")]
public class EntrySO : ScriptableObject
{
    public string entryName;

    public string entryAPI;

    public string explainText;

    public List<Option> optionList;
}