using UnityEngine;
using UnityEngine.UI;

public class Ability : MonoBehaviour
{
    [Header("Ability Data")]
    [SerializeField] private AbilitySO abilitySO;
    
    [Header("References")]
    [SerializeField] private Text abilityText;
    [SerializeField] private Image abilityImage;

    public void Init(AbilitySO so)
    {
        abilitySO = so;
        
        gameObject.name = abilitySO.abilityName;
        abilityText.text = abilitySO.abilityName;
        abilityImage.sprite = abilitySO.abilitySprite;
    }

    public void OnClick()
    {
        GameManager.Instance.LoadScene(abilitySO.abilitySceneName);
    }
}
