using UnityEngine;
using UnityEngine.UI;

public class ResultController : MonoBehaviour
{
    private Text _contentText;

    private void Awake()
    {
        _contentText = transform.Find("Text").GetComponent<Text>();
    }

    public void ChangeContent(string content)
    {
        _contentText.text = content;
    }
}