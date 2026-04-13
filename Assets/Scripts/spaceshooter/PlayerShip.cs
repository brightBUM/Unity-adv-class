using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerShip : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float xBound = 8.3f;
    [SerializeField] float bulletSpeed = 10f;
    [SerializeField] float bulletlifeTime = 1f;
    [SerializeField] float recoilAmount = 0.2f;
    [SerializeField] float recoilTime = 0.25f;
    [SerializeField] GameObject projectile;
    [SerializeField] Transform spawnTransform;
    [SerializeField] AudioSource shipAudioSource;
    [SerializeField] TextMeshProUGUI scoreText;
    Rigidbody2D rb;
    float xMove;

    int score;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        

        xMove = Input.GetAxis("Horizontal");

        //transform 

        //transform.Translate(Vector3.right* xMove * moveSpeed * Time.deltaTime);
        //transform.position = new Vector3
        //(
        //    Mathf.Clamp(transform.position.x, -xBound, xBound),
        //    transform.position.y,
        //    transform.position.z
        //);

        if(Input.GetKeyDown(KeyCode.Space))
        {
            //Debug.Break();
            GameObject spawnObject = Instantiate(projectile,spawnTransform.position,Quaternion.identity);
            Rigidbody2D spawnObjectRB = spawnObject.GetComponent<Rigidbody2D>();
            spawnObjectRB.linearVelocity = Vector2.up * bulletSpeed;
            Destroy(spawnObject, bulletlifeTime);

            //recoil visual
            StartCoroutine(RecoilFeedback());

            //sfx
            shipAudioSource.Play();
        }
        
    }
    //never call on update directly
    IEnumerator RecoilFeedback()
    {
        //move ship down
        transform.position += Vector3.down * recoilAmount;
        yield return new WaitForSeconds(recoilTime);
        transform.position += Vector3.up * recoilAmount;
       
        //move ship back to original y pos
    }
    private void FixedUpdate()
    {
        //rigidbody 
        rb.MovePosition((Vector3)rb.position + 
            Vector3.right * xMove * moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Coin"))
        {
            Destroy(collision.gameObject);
            score++;
            scoreText.text = score.ToString();
        }

    }
}
