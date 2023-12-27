using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EntrySO")]
public class EntrySO : ScriptableObject
{
    public string entryName;
    public string entryAPI;
    [TextArea]
    public string entryDescription;
    public string entryScriptTypeName;
    public List<Option> optionList;
    public string buttonText = "运行";

    private Type _entryScriptType;
    
    public Type EntryScriptType
    {
        get
        {
            if (_entryScriptType == null && !string.IsNullOrEmpty(entryScriptTypeName))
            {
                _entryScriptType = Type.GetType(entryScriptTypeName);
            }
            return _entryScriptType;
        }
        set
        {
            _entryScriptType = value;
            entryScriptTypeName = _entryScriptType?.FullName;
            // Debug.Log($"EntryScriptType changed to: {entryScriptTypeName}");
        }
    }
}