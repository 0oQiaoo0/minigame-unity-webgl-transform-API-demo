using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    [SerializeField]private GameObject _mainCanvas;
    [SerializeField]private GameObject _detailsCanvas;
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
        
        GetReferences();
        
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
        _mainCanvas.SetActive(true);
        _detailsCanvas.SetActive(false);
    }

    public void SwitchCanvas()
    {
        _isMainCanvasActive = !_isMainCanvasActive;
        _mainCanvas.SetActive(_isMainCanvasActive);
        _detailsCanvas.SetActive(!_isMainCanvasActive);
    }
    
    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        var asyncOperation = SceneManager.LoadSceneAsync(sceneName);

        // 等待场景加载完成
        while (!asyncOperation.isDone)
        {
            yield return null;
        }

        // 场景加载完成后执行的操作
        if (sceneName == "MainScene")
        {
            GetReferences();
        }
    }

    private void GetReferences()
    {
        _mainCanvas = GetSceneRootGO("Main Canvas");
        _detailsCanvas = GetSceneRootGO("Details Canvas");
        detailsController = _detailsCanvas.GetComponent<DetailsController>();
    }

    public GameObject GetSceneRootGO(string name)
    {
        // 获取当前场景中的所有根物体
        var rootObjects = SceneManager.GetActiveScene().GetRootGameObjects();
        // 遍历所有顶层游戏对象，找到名字为name的游戏对象则返回，否则返回null
        return rootObjects.FirstOrDefault(obj => obj.name == name);
    }
}