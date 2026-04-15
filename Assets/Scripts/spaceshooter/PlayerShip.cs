using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] List<Transform> spawnTransforms;
    [SerializeField] AudioSource shipAudioSource;
    [SerializeField] TextMeshProUGUI scoreText;
    Rigidbody2D rb;
    float xMove;
    List<List<int>> spawnPointGroup = new List<List<int>> 
    {
        new List<int>{0},
        new List<int>{1,2},
        new List<int>{0,1,2},
        new List<int>{1,2,3,4},
        new List<int>{0,1,2,3,4},

    };
    int spawnPointGroupIndex;
    int score;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spawnPointGroupIndex = 0;
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
            SpawnBullet();

            //recoil visual
            StartCoroutine(RecoilFeedback());

            //sfx
            shipAudioSource.Play();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            spawnPointGroupIndex++;

        }
    }

    private void SpawnBullet()
    {
        for (int i = 0; i < spawnPointGroup[spawnPointGroupIndex].Count; i++)
        {
            var j = spawnPointGroup[spawnPointGroupIndex][i];
            var spawnPos = spawnTransforms[j].position;
            GameObject spawnObject = Instantiate(projectile, spawnPos, Quaternion.identity);
            Rigidbody2D spawnObjectRB = spawnObject.GetComponent<Rigidbody2D>();
            spawnObjectRB.linearVelocity = Vector2.up * bulletSpeed;
            Destroy(spawnObject, bulletlifeTime);
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
