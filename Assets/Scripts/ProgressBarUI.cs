using System.Collections;
using UnityEngine;
using UnityEngine.UI;


//Should enable/disable
//updates the progressBar fill
//follow the player on XZ
public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] GameObject progressbarObject;
    [SerializeField] Image progressBar;
    [SerializeField] Transform targetTransform;
    [SerializeField] bool visibleOnHit;
    Vector3 offset;
    float timer = 2f;
    Coroutine disableCoroutine;
    private void Start()
    {
        offset = targetTransform.position - transform.position;
    }
    public void ToggleProgressBar(bool value)
    {
        progressbarObject.SetActive(value);
    }
    public void UpdateUIFillAmount(float amount)
    {
        if(!progressBar.IsActive())
            ToggleProgressBar(true);

        if(disableCoroutine!=null)
        {
            StopCoroutine(disableCoroutine);
        }
        progressBar.fillAmount = amount;
        disableCoroutine = StartCoroutine(DisableAfterTrigger());
    }

    IEnumerator DisableAfterTrigger()
    {
        yield return new WaitForSeconds(timer);
        ToggleProgressBar(false);
    }
    private void Update()
    {
        if(targetTransform!=null)
        {
            //transform.position = targetTransform.position - offset;
            transform.forward = Camera.main.transform.forward;
        }

        
    }
}
