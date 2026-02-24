using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI ammoText;
    [SerializeField] PlayerController playerController;
    private void OnEnable()
    {
        playerController.OnAmmoChangeAction += UpdateAmmoText;
    }

    private void UpdateAmmoText(string ammoText)
    {
        this.ammoText.text = ammoText;
    }
    private void OnDisable()
    {
        playerController.OnAmmoChangeAction -= UpdateAmmoText;

    }
}
