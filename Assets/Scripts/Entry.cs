using UnityEngine;
using UnityEngine.UI;

public class Entry : MonoBehaviour
{
    [SerializeField] private EntrySO entrySO;

    [SerializeField] private Text entryNameText;
    public void Init(EntrySO so)
    {
        entrySO = so;
        entryNameText.text = entrySO.entryName;
    }
    
    public void OnClick()
    {
        DetailsController.Instance.Init(entrySO);
        UIController.Instance.SwitchCanvas();
    }
}