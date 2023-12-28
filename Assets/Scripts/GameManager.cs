using System;
using UnityEngine;
using UnityEngine.Serialization;
using WeChatWASM;
using SystemInfo = WeChatWASM.SystemInfo;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [HideInInspector] public DetailsController detailsController;
    
    [Header("Font")]
    public Font font;
    public Action<Font> onFontLoaded;
    
    [Header("Canvas Switch")]
    [SerializeField] private GameObject mainCanvas;
    [SerializeField] private GameObject detailsCanvas;
    private bool _isMainCanvasActive = true;
    
    [Header("System Info")]
    public SystemInfo systemInfo;
    
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
                if (!font) return;
                
                this.font = font;
                onFontLoaded?.Invoke(font);
            });
            
            systemInfo = WX.GetSystemInfoSync();
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