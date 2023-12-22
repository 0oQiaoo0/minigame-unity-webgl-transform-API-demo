using UnityEngine;
using UnityEngine.UI;
using WeChatWASM;

public class TextInit : MonoBehaviour
{
    void Start()
    {
        var text = GetComponent<Text>();
        
        // fallbackFont作为旧版本微信或者无法获得系统字体文件时的备选CDN URL
        var fallbackFont = Application.streamingAssetsPath + "/Fz.ttf";
        WX.GetWXFont(fallbackFont, (font) =>
        {
            if (font)
            {
                text.font = font;
            }
        });
    }
}
