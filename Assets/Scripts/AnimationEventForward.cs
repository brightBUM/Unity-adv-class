using UnityEngine;
using UnityEngine.Events;

public class AnimationEventForward : MonoBehaviour
{
    public UnityEvent[] forwardEvent;

    public void TriggerAnimEvent(int value)
    {
        forwardEvent[value].Invoke();
    }
}
