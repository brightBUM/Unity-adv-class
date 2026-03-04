using System.Collections;
using UnityEngine;

public class Knockable : MonoBehaviour, IDamageable
{
    [SerializeField] float knockBackForce;
    [SerializeField] float fadeDuration = 2f;
    Rigidbody rb;
    Material material;
    Coroutine fadeCoroutine;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        material = GetComponent<MeshRenderer>().material;
    }
    public int Health { get ; set ; }

    public void Die()
    {
        throw new System.NotImplementedException();
    }

    public void TakeDamage(int amount, Vector3 hitPoint)
    {
        var knockDirection = transform.position - hitPoint;
        rb.AddForce(knockDirection.normalized * knockBackForce, ForceMode.Impulse);

        //colorFeedback
        if(fadeCoroutine!=null)
        {
            StopCoroutine(fadeCoroutine);
        }

        fadeCoroutine = StartCoroutine(HitFeedback());
       
    }

    IEnumerator HitFeedback()
    {
        float timer = 0f;
        var startColor = Color.red;

        while (timer<fadeDuration)
        {
            material.color = Color.Lerp(startColor, Color.white, timer/fadeDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        material.color = Color.white;

    }
}
