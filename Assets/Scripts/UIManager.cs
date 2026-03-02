using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI ammoText;
    
    private void OnEnable()
    {
        PlayerController.OnAmmoChangeAction += UpdateAmmoText;
    }

    private void UpdateAmmoText(string ammoText)
    {
        this.ammoText.text = ammoText;
    }
    private void OnDisable()
    {
        PlayerController.OnAmmoChangeAction -= UpdateAmmoText;

    }
}
