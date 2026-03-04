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
    [SerializeField] private float fireRate = 0.1f;

    [SerializeField] LayerMask damageLayer;
    [SerializeField] Animator animator;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] ProgressBarUI progressBarUI;
    [SerializeField] ParticleSystem muzzleFlashVFX;
    [SerializeField] GameObject bulletTracePrefab;
    [SerializeField] Transform shootTransform;
    [SerializeField] int magSize = 7;
    [SerializeField] float reloadTime = 2f;

    //private fields
    int count;
    float reloadTimer = 0f;
    private CharacterController characterController;
    Vector3 hitPoint;
    bool inRange;

    //public fields
    public static Action<string> OnAmmoChangeAction;
    
    private void OnEnable()
    {
        playerInput.moveAction += Movement;
        playerInput.lookAction += Rotation;
        playerInput.shootStartedAction += ShootStarted;
        playerInput.shootCancelledAction += ShootReleased;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        count = magSize;
    }
    Coroutine fireCoroutine;

    private void ShootStarted()
    {
        fireCoroutine = StartCoroutine(AutoFire());
    }

    private void ShootReleased()
    {
        if (fireCoroutine != null)
            StopCoroutine(fireCoroutine);
    }
    IEnumerator AutoFire()
    {
        while (true)
        {
            Shooting();
            yield return new WaitForSeconds(fireRate);
        }
    }
    private void Shooting()
    {
        if (count > 0)
        {
            muzzleFlashVFX.Play();
            var bulletTrace = Instantiate(bulletTracePrefab, shootTransform.position, Quaternion.identity);
            if(Physics.Raycast(transform.position,transform.forward, out RaycastHit hitInfo))
            {
                inRange = true;
                var damageable = hitInfo.collider.GetComponent<IDamageable>();
                if (damageable!=null)
                {
                    damageable.TakeDamage(25,hitInfo.point);
                }
                bulletTrace.GetComponent<BulletTrace>().Init(shootTransform.position, hitInfo.point,true);
                //Debug.Break();
            }
            else
            {
                inRange = false;
                bulletTrace.GetComponent<BulletTrace>().Init(shootTransform.position, shootTransform.position+transform.forward*50f,false);

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
        Vector3 worldMove = new Vector3(moveInput.x, 0, moveInput.y);
        //convert world to local w.r.t player transform
        Vector3 localMove = transform.InverseTransformDirection(worldMove);
        //local conversion only for the animator 
        animator.SetBool("move", worldMove.magnitude != 0f);
        animator.SetFloat("moveX", localMove.x,0.1f,Time.deltaTime);
        animator.SetFloat("moveY", localMove.z,0.1f, Time.deltaTime); //here Y is Z

        characterController.Move(worldMove * Time.deltaTime * moveSpeed);
    }

    private void OnDisable()
    {
        //moveInput.action.performed -= MoveInputPerformed;
        playerInput.moveAction -= Movement;
        playerInput.lookAction -= Rotation;
        playerInput.shootStartedAction += ShootStarted;
        playerInput.shootCancelledAction += ShootReleased;

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = inRange ? Color.green : Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * rayCastDistance);
    }
}
