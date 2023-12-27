using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TextInit : MonoBehaviour
{
    private Text _text;

    private void Awake()
    {
        _text = GetComponent<Text>();
    }

    private void Start()
    {
        if (GameManager.Instance.Font != null)
        {
            _text.font = GameManager.Instance.Font;
        }
        else
        {
            GameManager.Instance.OnFontLoaded += OnFontLoaded;
        }
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnFontLoaded -= OnFontLoaded;
    }

    private void OnFontLoaded(Font font)
    {
        _text.font = font;
    }
}
