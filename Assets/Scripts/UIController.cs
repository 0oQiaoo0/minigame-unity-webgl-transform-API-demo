using UnityEngine;

public class UIController : MonoBehaviour
{
    public static UIController Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
    }
    
    [SerializeField] private GameObject mainCanvas;
    [SerializeField] private GameObject detailsCanvas;
    private bool _isMainCanvasActive = true;
    
    public void SwitchCanvas()
    {
        _isMainCanvasActive = !_isMainCanvasActive;
        mainCanvas.SetActive(_isMainCanvasActive);
        detailsCanvas.SetActive(!_isMainCanvasActive);
    }
}