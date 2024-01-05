using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class BackToMainSceneButton : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }
    
    private void OnClick()
    {
        GameManager.Instance.LoadScene("MainScene");
    }
}
