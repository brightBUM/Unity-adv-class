using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStack : MonoBehaviour
{
    [SerializeField] Transform holdTransform;
    [SerializeField] Transform stackParent;
    [SerializeField] float speed = 3.0f;
    [SerializeField] float rotSpeed = 3.0f;
    [SerializeField] float collectSpeed = 10.0f;

    public List<Transform> stackedItems = new List<Transform>();
    Rigidbody rb;
    Vector3 move;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();

    }
    private void Update()
    {
        //movement
        move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        
    }
    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + move * Time.deltaTime * speed);
        if (move != Vector3.zero)
        {
            float angle = Mathf.Atan2(move.x, move.z) * Mathf.Rad2Deg;
            Quaternion rot = Quaternion.Euler(0, angle, 0);
            Quaternion smoothRot = Quaternion.Slerp(transform.rotation, rot, rotSpeed * Time.deltaTime);
            rb.MoveRotation(smoothRot);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7) // interact layer
        {
            if (other.transform.parent == stackParent) //to prevent while transfering
                return;

            Transform item = other.transform;

            item.SetParent(stackParent);

            item.GetComponent<Collider>().enabled = false;

            StartCoroutine(Move(item, holdTransform.localPosition));

            holdTransform.localPosition += Vector3.up * 0.6f;

            stackedItems.Add(item);
        }
    }

    public Transform RemoveTopItem()
    {
        if (stackedItems.Count == 0) return null;

        Transform item = stackedItems[stackedItems.Count-1];
        stackedItems.RemoveAt(stackedItems.Count - 1);

        holdTransform.localPosition -= Vector3.up * 0.6f;

        return item;
    }

    IEnumerator Move(Transform stackItem, Vector3 target)
    {
        while (Vector3.Distance(stackItem.localPosition, target) > 0.01f)
        {
            stackItem.localPosition = Vector3.MoveTowards(
                stackItem.localPosition,
                target,
                collectSpeed * Time.deltaTime);

            yield return null;
        }

        stackItem.localPosition = target;
        stackItem.rotation = holdTransform.rotation;
    }
}