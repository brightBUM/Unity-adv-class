using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] InputActionReference moveInput;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float rotSpeed = 2f;
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private float rayCastDistance = 10f;
    [SerializeField] LayerMask damageLayer;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] int magSize = 7;
    [SerializeField] float reloadTime = 2f;
    int count;
    private CharacterController characterController;
    Vector3 hitPoint;
    bool inRange;
    private void OnEnable()
    {
        moveInput.action.performed += MoveInputPerformed;

    }
   

    private void MoveInputPerformed(InputAction.CallbackContext obj)
    {
        

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
        //movement
        Movement();

        //Target - mouse in XZ plane
        Rotation();

        //shoot
        Shooting();

    }

    private void Shooting()
    {
        if (Input.GetMouseButtonDown(0) && count>0)
        {
            var bulletItem = Instantiate(bulletPrefab,
                                         transform.position + transform.forward,
                                         Quaternion.identity);
            bulletItem.GetComponent<Rigidbody>().linearVelocity = transform.forward * bulletSpeed;
            count--;
            if(count<=0)
            {
                //invoke reload
                StartCoroutine(Reload());
            }
        }
        inRange = Physics.Raycast(transform.position, transform.forward, rayCastDistance, damageLayer);
    }
    IEnumerator Reload()
    {
        yield return new WaitForSeconds(reloadTime);

        count = magSize;
    }
    private void Rotation()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
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

    private void Movement()
    {
        var xInput = Input.GetAxis("Horizontal");
        var zInput = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(xInput, 0, zInput);

        characterController.Move(move * Time.deltaTime * moveSpeed);
    }

    private void OnDisable()
    {
        moveInput.action.performed -= MoveInputPerformed;

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = inRange?Color.green:Color.red;
        Gizmos.DrawLine(transform.position, transform.forward*rayCastDistance);
    }

}
