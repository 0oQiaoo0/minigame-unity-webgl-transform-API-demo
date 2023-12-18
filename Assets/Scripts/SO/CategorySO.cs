using System.Collections.Generic;
using UnityEngine;

namespace SO
{
    [CreateAssetMenu(menuName = "CategorySO")]
    public class CategorySO : ScriptableObject
    {
        public string categoryName;
        
        public List<EntrySO> entryList;
    }
}
