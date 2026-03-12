using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepositStack : MonoBehaviour
{
    [SerializeField] Transform holdTransform;
    [SerializeField] Transform stackParent;
    [SerializeField] float transferSpeed = 10f;

    public List<Transform> depositedItems = new List<Transform>();

    PlayerStack currentPlayer;
    Coroutine transferRoutine;

    private void OnTriggerEnter(Collider other)
    {
        PlayerStack player = other.GetComponent<PlayerStack>();

        if (player == null) return;

        currentPlayer = player;

        // Only start if no transfer is already running
        if (transferRoutine == null)
            transferRoutine = StartCoroutine(TransferItems());
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerStack player = other.GetComponent<PlayerStack>();

        if (player == null) return;

        // Player left — stop transferring
        //if (transferRoutine != null)
        //{
        //    StopCoroutine(transferRoutine);
        //    transferRoutine = null;
        //}

        currentPlayer = null;
    }

    IEnumerator TransferItems()
    {
        while (currentPlayer != null)
        {
            Transform item = currentPlayer.RemoveTopItem();

            if (item == null)
            {
                transferRoutine = null;
                yield break;
            }

            item.SetParent(stackParent);

            Vector3 target = holdTransform.localPosition;

            yield return Move(item, target);

            holdTransform.localPosition += Vector3.up * 0.6f;

            depositedItems.Add(item);

            //item.GetComponent<Collider>().enabled = true;

            yield return new WaitForSeconds(0.05f);
        }

        transferRoutine = null;
    }

    IEnumerator Move(Transform stackItem, Vector3 target)
    {
        while (Vector3.Distance(stackItem.localPosition, target) > 0.01f)
        {
            stackItem.localPosition = Vector3.MoveTowards(
                stackItem.localPosition,
                target,
                transferSpeed * Time.deltaTime);

            yield return null;
        }

        stackItem.localPosition = target;
    }
}