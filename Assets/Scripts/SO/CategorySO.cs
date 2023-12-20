using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CategorySO")]
public class CategorySO : ScriptableObject
{
    public string categoryName;
    
    public List<EntrySO> entryList;
}