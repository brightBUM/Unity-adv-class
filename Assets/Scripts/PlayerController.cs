using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System;
public class PlayerController : MonoBehaviour
{
    [SerializeField] PlayerInput playerInput;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float rotSpeed = 2f;
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private float rayCastDistance = 10f;
    [SerializeField] LayerMask damageLayer;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] ProgressBarUI progressBarUI;
    [SerializeField] int magSize = 7;
    [SerializeField] float reloadTime = 2f;

    //private fields
    int count;
    float reloadTimer = 0f;
    private CharacterController characterController;
    Vector3 hitPoint;
    bool inRange;

    //public fields
    public Action<string> OnAmmoChangeAction;
    
    private void OnEnable()
    {
        playerInput.moveAction += Movement;
        playerInput.lookAction += Rotation;
        playerInput.shootAction += Shooting;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        count = magSize;
    }

    // Update is called once per frame
    void Update()
    {
        

    }

    private void Shooting()
    {
        if (count > 0)
        {
            //var bulletItem = Instantiate(bulletPrefab,
            //                             transform.position + transform.forward,
            //                             Quaternion.identity);
            //bulletItem.GetComponent<Rigidbody>().linearVelocity = transform.forward * bulletSpeed;

            
            //inRange = Physics.Raycast(transform.position, transform.forward, rayCastDistance, damageLayer);
            
            if(Physics.Raycast(transform.position,transform.forward, out RaycastHit hitInfo))
            {
                inRange = true;
                var damageable = hitInfo.collider.GetComponent<IDamageable>();
                if (damageable!=null)
                {
                    damageable.TakeDamage(10);
                }
            }
            else
            {
                inRange = false;
            }

            count--;
            UpdateAmmoUI();

            if (count <= 0)
            {
                //invoke reload
                StartCoroutine(Reload());
            }
        }
        
    }

    private void UpdateAmmoUI()
    {
        string ammoText = count + "/" + magSize;
        OnAmmoChangeAction.Invoke(ammoText);
    }

    IEnumerator Reload()
    {
        reloadTimer = 0f;
        progressBarUI.ToggleProgressBar(true);

        while (reloadTimer <= reloadTime)
        {
            reloadTimer += Time.deltaTime;
            progressBarUI.UpdateUIFillAmount(reloadTimer / reloadTime);

            yield return null; //skips the frame / executes 1 frame
        }

        count = magSize;
        progressBarUI.ToggleProgressBar(false);
        UpdateAmmoUI();

    }
    private void Rotation(Vector2 mousePosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        if (groundPlane.Raycast(ray, out float enter))
        {
            hitPoint = ray.GetPoint(enter);
            //dummyTransform.position = hitPoint;

            //rotation
            Vector3 direction = hitPoint - transform.position;
            direction.y = 0;
            float angleinRadians = Mathf.Atan2(direction.x, direction.z);
            float angleinDegrees = angleinRadians * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Euler(0, angleinDegrees, 0);
        }
    }

    private void Movement(Vector2 moveInput)
    {
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);

        characterController.Move(move * Time.deltaTime * moveSpeed);
    }

    private void OnDisable()
    {
        //moveInput.action.performed -= MoveInputPerformed;
        playerInput.moveAction -= Movement;
        playerInput.lookAction -= Rotation;
        playerInput.shootAction -= Shooting;

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = inRange ? Color.green : Color.red;
        Gizmos.DrawLine(transform.position, transform.forward * rayCastDistance);
    }
}
