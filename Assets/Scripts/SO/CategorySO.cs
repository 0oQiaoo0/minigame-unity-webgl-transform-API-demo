using System.Collections.Generic;
using UnityEngine;

namespace SO
{
    public class CategorySO : ScriptableObject
    {
        public string categoryName;
        
        public List<EntrySO> entryList;
    }
}
