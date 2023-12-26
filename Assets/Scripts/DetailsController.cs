using UnityEngine;

public class DetailsController : MonoBehaviour
{
    [SerializeField] private EntrySO entrySO;
    
    private void ClearDetails()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
    
    public void Init(EntrySO so)
    {
        entrySO = so;
    }
}
