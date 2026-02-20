using UnityEngine;
using UnityEngine.UI;


//Should enable/disable
//updates the progressBar fill
//follow the player on XZ
public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] GameObject progressbarObject;
    [SerializeField] Image progressBar;
    [SerializeField] Transform playerTransform;
    Vector3 offset;
    private void Start()
    {
        offset = playerTransform.position - transform.position;
    }
    public void ToggleProgressBar(bool value)
    {
        progressbarObject.SetActive(value);
    }
    public void UpdateUIFillAmount(float amount)
    {
        progressBar.fillAmount = amount;
    }
    private void Update()
    {
        transform.position = playerTransform.position - offset;
    }
}
