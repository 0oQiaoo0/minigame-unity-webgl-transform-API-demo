using UnityEngine;
using UnityEngine.UI;

public class ResultController : MonoBehaviour
{
    private GameObject _titleGameObject;
    private Text _titleText;
    private GameObject _contentGameObject;
    private Text _contentText;

    private void Awake()
    {
        _titleGameObject = transform.Find("Result Title").gameObject;
        _titleText = transform.Find("Result Title").Find("Text").GetComponent<Text>();
        _contentGameObject = transform.Find("Result Content").gameObject;
        _contentText = transform.Find("Result Content").Find("Text").GetComponent<Text>();
    }

    public void ChangeTitle(string title)
    {
        _titleGameObject.SetActive(!string.IsNullOrEmpty(title));
        _titleText.text = title;
    }
    
    public void ChangeContent(string content)
    {
        _contentGameObject.SetActive(!string.IsNullOrEmpty(content));
        _contentText.text = content;
    }
}