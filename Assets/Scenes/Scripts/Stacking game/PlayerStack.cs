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

    [Header("Knock Settings")]
    [SerializeField] float knockUpForce = 4f;
    [SerializeField] float knockSpreadForce = 2f;

    public List<Transform> stackedItems = new List<Transform>();
    Rigidbody rb;
    Vector3 move;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
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
        else
        {
            rb.angularVelocity = Vector3.zero;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7) // interact layer
        {
            if (other.transform.parent == stackParent)
                return;

            Transform item = other.transform;

            // Disable rigidbody physics while carried — make kinematic
            Rigidbody itemRb = item.GetComponent<Rigidbody>();
            if (itemRb != null)
            {
                itemRb.linearVelocity = Vector3.zero;
                itemRb.angularVelocity = Vector3.zero;
                itemRb.isKinematic = true;
            }

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
        stackedItems.RemoveAt(stackedItems.Count-1);

        holdTransform.localPosition -= Vector3.up * 0.6f;

        return item;
    }

    // Called by an obstacle or knockback source to scatter all carried items
    [ContextMenu("knockStack")]
    public void KnockStack()
    {
        foreach (Transform item in stackedItems)
        {
            DropItem(item);
        }
        stackedItems.Clear();

        // Reset hold position
        holdTransform.localPosition = Vector3.zero;
    }

    void DropItem(Transform item)
    {
        item.SetParent(null);

        Rigidbody itemRb = item.GetComponent<Rigidbody>();
        if (itemRb != null)
        {
            itemRb.isKinematic = false;

            // Random spread direction + upward force
            Vector3 randomDir = new Vector3(
                Random.Range(-1f, 1f),
                0,
                Random.Range(-1f, 1f)).normalized;

            itemRb.AddForce(randomDir * knockSpreadForce + Vector3.up * knockUpForce, ForceMode.Impulse);
        }

        // Delay before item can be re-collected — lets it fly and land first
        item.gameObject.layer = 0;
        item.GetComponent<Collider>().enabled = true;

        StartCoroutine(EnableAfterDelay(item, 1.5f));
    }
    IEnumerator EnableAfterDelay(Transform item, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (item == null) yield break;

        item.gameObject.layer = 7;
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