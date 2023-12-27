using System;
using UnityEngine;
using WeChatWASM;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    public Font Font { get; private set; }

    public event Action<Font> OnFontLoaded;
    
    [SerializeField] private GameObject mainCanvas;
    [SerializeField] private GameObject detailsCanvas;
    private bool _isMainCanvasActive = true;
    
    public DetailsController detailsController;
    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        
        Instance = this;
        
        DontDestroyOnLoad(gameObject);
        
        detailsController = detailsCanvas.GetComponent<DetailsController>();
        
        WX.InitSDK((code) =>
        {
            Debug.Log("InitSDK: " + code);

            var fallbackFont = Application.streamingAssetsPath + "/Fz.ttf";
            WX.GetWXFont(fallbackFont, (font) =>
            {
                if (font)
                {
                    Font = font;
                    OnFontLoaded?.Invoke(font);
                }
            });
        });
    }
    
    private void Start()
    {
        mainCanvas.SetActive(true);
        detailsCanvas.SetActive(false);
    }

    public void SwitchCanvas()
    {
        _isMainCanvasActive = !_isMainCanvasActive;
        mainCanvas.SetActive(_isMainCanvasActive);
        detailsCanvas.SetActive(!_isMainCanvasActive);
    }
}